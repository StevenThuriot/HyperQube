using System;
using System.Windows.Forms;
using HyperQube.Library;

namespace HyperQube
{
    internal class TooltipProvider : ITooltipProvider
    {
        public NotifyIcon Icon { get; set; }

        public void ShowTooltip(string title, string message, QubeIcon icon = QubeIcon.Info)
        {
            var notifyIcon = Icon;
            if (notifyIcon == null) return;

            notifyIcon.ShowBalloonTip(1500, title, message, MapIconToWinforms(icon));
        }

        private static ToolTipIcon MapIconToWinforms(QubeIcon icon)
        {
            ToolTipIcon toolTipIcon;
            switch (icon)
            {
                case QubeIcon.None:
                    toolTipIcon = ToolTipIcon.None;
                    break;
                case QubeIcon.Info:
                    toolTipIcon = ToolTipIcon.Info;
                    break;
                case QubeIcon.Warning:
                    toolTipIcon = ToolTipIcon.Warning;
                    break;
                case QubeIcon.Error:
                    toolTipIcon = ToolTipIcon.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("icon");
            }
            return toolTipIcon;
        }

        public void ShowIcon()
        {
            var notifyIcon = Icon;
            if (notifyIcon == null) return;

            notifyIcon.Visible = true;
        }

        public void HideIcon()
        {
            var notifyIcon = Icon;
            if (notifyIcon == null) return;

            notifyIcon.Visible = false;
        }
    }
}
