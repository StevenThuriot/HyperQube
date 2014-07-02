using System;

namespace HyperQube.Library
{
    [Flags]
    public enum Interests
    {
        None = 0,

        Note = 1 << 1,
        Address = 1 << 2,
        List = 1 << 3,
        File = 1 << 4,
        Link = 1 << 5,
        Mirror = 1 << 6,

        All = Note | Address | List | File | Link | Mirror
    }
}