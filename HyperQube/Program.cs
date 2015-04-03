using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading;
using System.Windows.Forms;
using HyperQube.Library;

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
