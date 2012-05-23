using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public class DXFBlock : DXFGroup
    {
        public string Name;
        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 2:
                    Name = Converter.FValue;
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
    }
}
