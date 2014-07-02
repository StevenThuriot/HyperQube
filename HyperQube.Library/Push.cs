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
