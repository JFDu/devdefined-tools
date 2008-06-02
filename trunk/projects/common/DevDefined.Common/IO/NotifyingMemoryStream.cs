using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DevDefined.Common.IO
{
    public class NotifyingMemoryStream : NotifyingStream
    {
        public NotifyingMemoryStream()
            : base(new MemoryStream())
        {
        }

        public NotifyingMemoryStream(byte[] buffer)
            : base(new MemoryStream(buffer))
        { 
        }

        public byte[] ToArray()
        {
            return ((MemoryStream)InnerStream).ToArray();
        }
    }
}
