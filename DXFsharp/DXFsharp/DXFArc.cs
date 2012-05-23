using System;
using System.Drawing;

namespace DXFsharp
{
    public class DXFArc : DXFCircle
    {
        public float startAngle;
        public float endAngle;
        public SFPoint pt1;
        public SFPoint pt2;

        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 50:
                    startAngle = Converter.FloatValue();
                    break;
                case 51:
                    endAngle = Converter.FloatValue();
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
        public override void Loaded()
        {
            //closed = false; //sm
            base.Loaded();
            pt1 = RotPt(startAngle);
            pt2 = RotPt(endAngle);
            //DXFConst.DoExtrusion(pt1, extrusion); //sm
            //DXFConst.DoExtrusion(pt2, extrusion); //sm
        }
        private SFPoint RotPt(float Angle)
        {
            SFPoint Result;
            Angle = (float)DXFConst.Radian(Angle);
            Result.X = this.Point1.X + (float)(radius * Math.Cos(Angle));
            Result.Y = this.Point1.Y + (float)(radius * Math.Sin(Angle));
            Result.Z = this.Point1.Z;
            return Result;
        }
        public override void Params()
        {
            base.Params();
            AStart = startAngle - (float)(Math.Round(startAngle / 360) * 360);
            AEnd = endAngle - (float)(Math.Round(endAngle / 360) * 360);
            if (AEnd <= AStart) AEnd = AEnd + 360;

        }
        public override void Draw(Graphics G)
        {
            SFPoint P1;
            float rd1 = radius;
            P1 = Converter.GetPoint(Point1);
            rd1 = rd1 * Converter.FScale;
            P1.X = P1.X - rd1;
            P1.Y = P1.Y - rd1;
            Color RealColor = DXFConst.EntColor(this, Converter.FParams.Insert);
            float sA = -startAngle, eA = -endAngle;
            if (endAngle < startAngle) sA = Conversion_Angle(sA);
            eA -= sA;
            if (FVisible)
                G.DrawArc(new Pen(RealColor, 1), P1.X, P1.Y, rd1 * 2, rd1 * 2, sA, eA);
        }
        public float Conversion_Angle(float Val)
        {
            while (Val < 0) Val = Val + 360;
            return Val;
        }
    }
}
