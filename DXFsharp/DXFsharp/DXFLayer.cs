using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DXFsharp
{
    public class DXFLayer: DXFEntity
    {
        public Color color = DXFConst.clByLayer;
        public byte flags;
        public bool visible;
        public string name = "";

        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 70:
                    flags = (byte)Converter.FloatValue();
                    break;
                case 2:
                    name = "" + Converter.FValue;
                    break;
                case 62:
                    color = CADImage.IntToColor(Convert.ToInt32(Converter.FValue, Converter.N));
                    break;
            }
        }
        public override void Loaded()
        {
            /*if(color) {    // invisible
                color = CADImage.IntToColor(color.ToArgb());
                flags = (byte)(flags | 1);
            }*/
            if ((flags & 1) == 0) visible = true;
            else visible = false;
        }
    }
}
