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
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Windows.Forms;
using HyperQube.Library;
using System.IO;

namespace HyperQube
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            bool createdNew;
            using (new Mutex(false, "#HyperQube~BD0757E3-09DC-4B49-8285-3C495391F2E5#", out createdNew))
            {
                if (!createdNew) return; //Already running.

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                using (var container = Container.CreateCompositionContainer())
                {
                    string apiKey;
                    if (!TryGetApiKey(container, out apiKey)) return;

                    var connectionService = container.GetExportedValue<IConnectionService>();
                    using (var connection = connectionService.OpenConnection(apiKey))
                    {
                        container.ComposeExportedValue("Connection", connection);

                        //We are registering our wrapper here because we have a dependency loop otherwise.
                        ITooltipProvider taskBarIcon = new TooltipProvider();
                        taskBarIcon.RegisterInstance();

                        using (var context = container.GetExportedValue<ApplicationContext>())
                        {
                            Application.Run(context);
                        }
                    }
                }
            }
        }

        private static bool TryGetApiKey(ExportProvider container, out string apiKey)
        {
            var configurationProvider = container.GetExportedValue<IConfigurationProvider>();
            apiKey = configurationProvider.GetAPIKey();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                var inputProvider = container.GetExportedValue<IInputProvider>();
                var question = new ApiQuestion();

                if (!inputProvider.Ask("PushBullet", "Configure WebSocket.", question))
                    return false;

                apiKey = question.Result;
                configurationProvider.SetAPIKey(apiKey);
                configurationProvider.Save();
            }

            return true;
        }
    }
}
