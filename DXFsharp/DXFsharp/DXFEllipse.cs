using System;
using System.Drawing;

namespace DXFsharp
{
    public class DXFEllipse : DXFArc
    {
        public float angle;
        public SFPoint radPt;
        public float ratio;

        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 11:
                    radPt.X = Converter.FloatValue();
                    break;
                case 21:
                    radPt.Y = Converter.FloatValue();
                    break;
                case 31:
                    radPt.Z = Converter.FloatValue();
                    break;
                case 40:
                    ratio = Converter.FloatValue();
                    break;
                case 41:
                    startAngle = (float)(Converter.FloatValue() * 180 / Math.PI);
                    break;
                case 42:
                    endAngle = (float)(Converter.FloatValue() * 180 / Math.PI);
                    break;
                case 50:
                case 51:
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
        public override void Loaded()
        {
            /*if(extrusion.Z < 0) 
            {
                SwapInts(startAngle, endAngle);
                startAngle = -startAngle;
                endAngle = -endAngle;
            }*/
            radius = (float)Math.Sqrt(radPt.X * radPt.X + radPt.Y * radPt.Y);
            angle = (float)Math.Atan2(radPt.Y, radPt.X);
            base.Loaded();
        }
        public override void Params()
        {
            base.Params();
            B = A * ratio;
            if (Math.Abs(radius) > 1E-10)
            {
                ASin = radPt.Y / radius;
                ACos = radPt.X / radius;
            }
            else
            {
                ASin = 1.0F;
                ACos = 0.0F;
            }
        }
        public override void Draw(Graphics G)
        {
            SFPoint P1;
            float rd1, rd2;
            if (radPt.X == 0)
            {
                rd1 = Math.Abs(radius * ratio);
                rd2 = radius;
            }
            else
            {
                rd1 = radius;
                rd2 = Math.Abs(radius * ratio);
            }
            P1 = Converter.GetPoint(Point1);
            rd1 = rd1 * Converter.FScale;
            rd2 = rd2 * Converter.FScale;
            P1.X = P1.X - rd1;
            P1.Y = P1.Y - rd2;
            Color RealColor = DXFConst.EntColor(this, Converter.FParams.Insert);
            if (RealColor == DXFConst.clNone) RealColor = Color.Black;
            float sA = startAngle, eA = endAngle;
            //if(endAngle < startAngle) sA = Conversion_Angle(sA);
            eA -= sA;
            if (FVisible)
            {
                if (eA == 0) G.DrawEllipse(new Pen(RealColor, 1), P1.X, P1.Y, rd1 * 2, rd2 * 2);
                else G.DrawArc(new Pen(RealColor, 1), P1.X, P1.Y, rd1 * 2, rd2 * 2, 0, 360);//sA, eA);
            }
        }
    }
}
