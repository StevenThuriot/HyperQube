namespace HyperQube.Library.Extensions
{
    public static class Push
    {
        public static dynamic GetBody(dynamic json)
        {
            string type = json.type;

            switch (type.AsInterest())
            {
                case Interests.Note:
                case Interests.Mirror:
                    return json.body;

                case Interests.Address:
                    return json.address;

                case Interests.List:
                    return json.items;

                case Interests.File:
                    return json.file_url;

                case Interests.Link:
                    return json.url;

                default:
                    return json;
            }
        }


        /// <remarks>This method only supports single flags.</remarks>
        public static string AsPushType(this Interests interest)
        {
            if ((interest & Interests.Note) == Interests.Note)
            {
                return PushTypes.Note;
            }

            if ((interest & Interests.Address) == Interests.Address)
            {
                return PushTypes.Address;
            }

            if ((interest & Interests.List) == Interests.List)
            {
                return PushTypes.List;
            }

            if ((interest & Interests.File) == Interests.File)
            {
                return PushTypes.File;
            }

            if ((interest & Interests.Link) == Interests.Link)
            {
                return PushTypes.Link;
            }

            if ((interest & Interests.Mirror) == Interests.Mirror)
            {
                return PushTypes.Mirror;
            }

            return "";
        }

        public static Interests AsInterest(this string type)
        {
            switch (type)
            {
                case PushTypes.Note:
                    return Interests.Note;
                case PushTypes.Address:
                    return Interests.Address;
                case PushTypes.List:
                    return Interests.List;
                case PushTypes.File:
                    return Interests.File;
                case PushTypes.Link:
                    return Interests.Link;
                case PushTypes.Mirror:
                    return Interests.Mirror;

                default:
                    return Interests.None;
            }
        }
    }
}
