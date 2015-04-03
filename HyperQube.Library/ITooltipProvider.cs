namespace HyperQube.Library
{
    public interface ITooltipProvider
    {
        void ShowTooltip(string title, string message, QubeIcon icon = QubeIcon.Info);
    }
}
