using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace OxygenChamber.Server
{
    public class Class2 : PipelineFilterBase<Class1>
    {
        public override Class1 Filter(ref SequenceReader<byte> reader)
        {
            throw new NotImplementedException();
        }
    }
}
