using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBApi
{
    public interface IDecoder
    {
        decimal ReadDecimal();
        decimal ReadDecimalMax();
        long ReadLong();
        int ReadInt();
        int ReadIntMax();
        bool ReadBoolFromInt();
        string ReadString();
    }
}
