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
