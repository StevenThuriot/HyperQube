#region License

//  Copyright 2014 Steven Thuriot
//   
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using HyperQube.Library.Extensions;
using Newtonsoft.Json;
using HyperQube.Library;
using HyperQube.Library.Questions;
using WebSocketSharp;

namespace HyperQube.Services
{
    [Export(typeof (ISubscriptionService))]
    public class SubscriptionService : ISubscriptionService
    {
        const string DisabledQubesKey = "DisabledQubes";
        const string LastTickle = "LastTickle";


        private readonly IOutputProvider _outputProvider;
        private readonly IInputProvider _inputProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ReadOnlyCollection<IQube> _qubes;

        private readonly List<IQube> _enabledQubes = new List<IQube>();
        private readonly Dictionary<IQube, IEnumerable<IDisposable>> _subscriptions = new Dictionary<IQube, IEnumerable<IDisposable>>();

        private bool _disposed;

        private IDisposable _errorSubscription;
        private IDisposable _socketSubscription;
        private IDisposable _tickleSubscription;

        private readonly ISubject<dynamic> _subject = new Subject<dynamic>();

        [ImportingConstructor]
        public SubscriptionService([Import("Connection", typeof(IDisposable))]WebSocket socket, IOutputProvider outputProvider, IInputProvider inputProvider, IConfigurationProvider configurationProvider, [ImportMany] IEnumerable<IQube> qubes)
        {
            _outputProvider = outputProvider;
            _inputProvider = inputProvider;
            _configurationProvider = configurationProvider;

            HookUpSocket(socket);

            _qubes = qubes.OrderBy(x => x.Title).ToList().AsReadOnly();

            var fullnames = _configurationProvider.GetValue(DisabledQubesKey);
            var disabledQubes = string.IsNullOrWhiteSpace(fullnames)
                                    ? new string[0]
                                    : fullnames.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var qube in _qubes.Where(qube => !disabledQubes.Contains(qube.GetType().FullName)))
            {
                Subscribe(qube);
                _enabledQubes.Add(qube);
            }
        }

        public async void EnableOrDisableQubes()
        {
            var mapping = new Dictionary<Guid, dynamic>();

            var questions = new List<BooleanQuestion>();
            foreach (var qube in _qubes)
            {
                var enabled = _enabledQubes.Contains(qube);

                var question = new BooleanQuestion(qube.Title, initialValue: enabled, type: QuestionType.Togglable);

                mapping.Add(question.Tag, new {Qube = qube, Enabled = enabled});

                questions.Add(question);
            }

            if (_inputProvider.Ask("Settings", "Enable or disable qubes.", questions.ToArray<IQuestion>()))
            {
                var disabledQubes = new List<IQube>();

                foreach (var question in questions)
                {
                    var map = mapping[question.Tag];

                    IQube qube = map.Qube;
                    bool isEnabled = map.Enabled;

                    if (question.Result)
                    {
                        if (!isEnabled)
                        {
                            Subscribe(qube);
                            _enabledQubes.Add(qube);
                        }
                    }
                    else
                    {
                        if (isEnabled)
                        {
                            foreach (var subscription in _subscriptions[qube])
                                subscription.Dispose();

                            _subscriptions.Remove(qube);
                            _enabledQubes.Remove(qube);
                        }

                        disabledQubes.Add(qube);
                    }

                    if (disabledQubes.Count > 0)
                    {
                        var fullnames = string.Join(";", disabledQubes.Select(x => x.GetType()).Select(x => x.FullName));
                        await _configurationProvider.SetValueAsync(DisabledQubesKey, fullnames);
                    }
                    else
                    {
                        await _configurationProvider.RemoveAsync(DisabledQubesKey);
                    }

                    await _configurationProvider.SaveAsync();
                }
            }
        }

        
        public void Subscribe(IQube qube)
        {
            var subscriptions = new List<IDisposable>();
            
            var qubeInterests = qube.Interests;
            if ((qubeInterests & Interests.All) == Interests.All)
            {
                var subscription = _subject.Subscribe(qube.Receive);
                subscriptions.Add(subscription);
            }
            else
            {
                var interests = Enum.GetValues(typeof (Interests))
                    .Cast<Interests>()
                    .Where(x => x != Interests.None && x != Interests.All)
                    .Where(x => (qubeInterests & x) == x)
                    .Select(x => x.AsPushType());


                foreach (var interest in interests)
                {
                    var copy = interest;

                    var observable = _subject.Where(json => StringComparer.OrdinalIgnoreCase.Equals((string) json.type, copy));
                    var subscription = observable.Subscribe(qube.Receive);

                    subscriptions.Add(subscription);
                }
            }

            _subscriptions[qube] = subscriptions;
        }




        private async void Tickle()
        {
            var lastModified = await _configurationProvider.GetValueAsync(LastTickle);

            var address = @"https://api.pushbullet.com/v2/pushes";
            
            decimal lastModifiedAsDecimal;
            if (string.IsNullOrWhiteSpace(lastModified))
            {
                lastModifiedAsDecimal = -1;
            }
            else
            {
                address += "?modified_after=" + lastModified;
                if (!decimal.TryParse(lastModified, out lastModifiedAsDecimal))
                    lastModifiedAsDecimal = -1;
            }

            using (var client = new WebClient())
            {
                var apiKey = await _configurationProvider.GetAPIKeyAsync();
                client.Headers["Authorization"] = "Bearer " + apiKey;

                var jsonString = await client.DownloadStringTaskAsync(address);
                
                var json = JsonConvert.DeserializeObject<dynamic>(jsonString);
                IEnumerable<dynamic> pushes = json.pushes;

                if (!pushes.Any()) return;

                var filteredPushes = pushes.Where(push => push.type != null)
                                           .Reverse() //Reverse so we start with oldest push.
                                           .ToList();

                for (var i = 0; i < filteredPushes.Count; i++)
                {
                    var push = filteredPushes[i];
                    
                    Observe((object) push);

                    if (i + 1 != filteredPushes.Count) continue;

                    var modified = push.modified; //Last modified push
                    if (modified == null) continue;

                    string modifiedAsString = modified.ToString();
                    decimal modifiedAsDecimal;
                    if (decimal.TryParse(modifiedAsString, out modifiedAsDecimal) && modifiedAsDecimal > lastModifiedAsDecimal)
                    {
                        lastModifiedAsDecimal = modifiedAsDecimal;
                    }
                }
            }

            if (lastModifiedAsDecimal == -1) return;
            
            await _configurationProvider.SetValueAsync(LastTickle, lastModifiedAsDecimal.ToString(CultureInfo.InvariantCulture));
            await _configurationProvider.SaveAsync();
        }

        private void Observe(object push)
        {
            _subject.OnNext(push);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void HookUpSocket(WebSocket socket)
        {
            _outputProvider.Trace("Hooking up events");

            _errorSubscription =
                Observable.FromEventPattern<ErrorEventArgs>(ev => socket.OnError += ev, ev => socket.OnError -= ev)
                          .Select(x => x.EventArgs.Message)
                          .Subscribe(OnError);

            var socketObserver = Observable.FromEventPattern<MessageEventArgs>(ev => socket.OnMessage += ev, ev => socket.OnMessage -= ev)
                                           .Select(x => x.EventArgs.Data)
                                           .Where(x => x != "{\"type\": \"nop\"}") //Ignore heartbeat, check as string so we don't spend time on deserializing it.
                                           .Select(JsonConvert.DeserializeObject<dynamic>);

            _socketSubscription = socketObserver.Where(x => x.type == "push") // New push
                                                .Select(x => x.push) //JSON Data from the push
                                                .Subscribe(Observe);

            _tickleSubscription = socketObserver.Where(x => x.type == "tickle" && x.subtype == "push")
                                                .Subscribe(_ => Tickle());
        }

        private void OnError(string exception)
        {
            _outputProvider.TraceError(exception);
        }


        ~SubscriptionService()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_errorSubscription != null)
                {
                    _errorSubscription.Dispose();
                    _errorSubscription = null;
                }

                if (_socketSubscription != null)
                {
                    _socketSubscription.Dispose();
                    _socketSubscription = null;
                }

                if (_tickleSubscription != null)
                {
                    _tickleSubscription.Dispose();
                    _tickleSubscription = null;
                }

                foreach (var subscription in _subscriptions.Values.SelectMany(x => x))
                    subscription.Dispose();

                _subscriptions.Clear();
            }

            _disposed = true;
        }
    }
}
