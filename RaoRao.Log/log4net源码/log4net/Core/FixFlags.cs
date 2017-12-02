namespace log4net.Core
{
    using System;

    [Flags]
    public enum FixFlags
    {
        All = 0xfffffff,
        Domain = 0x40,
        Exception = 0x100,
        Identity = 0x80,
        LocationInfo = 0x10,
        [Obsolete("Replaced by composite Properties")]
        Mdc = 1,
        Message = 4,
        Ndc = 2,
        None = 0,
        Partial = 0x34c,
        Properties = 0x200,
        ThreadName = 8,
        UserName = 0x20
    }
}

