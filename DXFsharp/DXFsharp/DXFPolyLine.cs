using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public class DXFPolyLine : DXFGroup
    {
        public float startW;
        public float endW;
        public float globalW;
        public float lineTypeScale;
        public int flags;
        public bool closed;
        public int meshM;
        public int meshN;

        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 40:
                    startW = Converter.FloatValue();
                    break;
                case 41:
                    endW = Converter.FloatValue();
                    break;
                case 43:
                    globalW = Converter.FloatValue();
                    if (globalW < DXFConst.accuracy) globalW = 0;
                    break;
                case 48:
                    lineTypeScale = Converter.FloatValue();
                    break;
                case 70:
                    flags = Converter.IntValue();
                    closed = (flags & 1) != 0;
                    break;
                case 71:
                    if (meshM == 0) meshM = Converter.IntValue();
                    break;
                case 73:
                    meshM = Converter.IntValue();
                    break;
                case 72:
                    if (meshN == 0) meshN = Converter.IntValue();
                    break;
                case 74:
                    meshN = Converter.IntValue();
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
    }
}
