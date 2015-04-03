namespace HyperQube.Library
{
    public static class ValidationMessages
    {
        public const string IsRequired = "{title} is a required field.";
        public const string InvalidFormat = "Invalid {title} format.";
        public const string InvalidUri = "'{value}' is not a valid Uri.";
        public const string NumbersOnly = "{title} can only contain numbers.";
        public const string InvalidPort = "'{value}' is not a valid port number.";
        public const string Whitespace = "Whitespace is not allowed in the '{title}' field.";
        public const string LineBreaks = "Line breaks are not allowed in the '{title}' field.";
    }
}