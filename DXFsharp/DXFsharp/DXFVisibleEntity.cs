using System;

namespace DXFsharp
{
    public class DXFVisibleEntity : DXFEntity
    {
        public DXFsharp.SFPoint Point1 = new SFPoint();
        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                //Layer
                case 8:
                    layer = Converter.LayerByName(Converter.FValue);
                    //if(layer != null) layer.used = true;
                    break;
                //Coordinates
                case 10:
                    Point1.X = Convert.ToSingle(Converter.FValue, Converter.N);
                    break;
                case 20:
                    Point1.Y = Convert.ToSingle(Converter.FValue, Converter.N);
                    break;
                //Color
                case 62:
                    FColor = CADImage.IntToColor(Convert.ToInt32(Converter.FValue, Converter.N));
                    break;
            }
        }
    }
}
