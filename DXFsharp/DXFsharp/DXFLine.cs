using System;
using System.Drawing;

namespace DXFsharp
{
    public class DXFLine :DXFVisibleEntity
    {
        public DXFsharp.SFPoint Point2 = new SFPoint();
        public override void Draw(System.Drawing.Graphics G)
        {
            SFPoint P1, P2;
            Color RealColor = DXFConst.EntColor(this, Converter.FParams.Insert);
            P1 = Converter.GetPoint(Point1);
            P2 = Converter.GetPoint(Point2);
            if (FVisible)
                G.DrawLine(new Pen(RealColor, 1), P1.X, P1.Y, P2.X, P2.Y);
        }

        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 11:
                    Point2.X = Convert.ToSingle(Converter.FValue, Converter.N);
                    break;
                case 21:
                    Point2.Y = Convert.ToSingle(Converter.FValue, Converter.N);
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
    }
}
