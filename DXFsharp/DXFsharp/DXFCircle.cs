using System;
using System.Drawing;

namespace DXFsharp
{
    public class DXFCircle : DXFVisibleEntity
    {
        protected float A, B, AStart, AEnd, ASin, ACos;
		public float radius;
		public override void ReadProperty()
		{
			if(Converter.FCode == 40) 
				radius = Convert.ToSingle(Converter.FValue, Converter.N);
			else
				base.ReadProperty();
		}
		public override void Loaded()
		{
			/*
			float S, C, delta;
			int I, N;
			DXFVertex V;
			numberOfParts = Converter.NumberOfPartsInCircle;
			entities.Clear();
			if(!(this is DXFArc)) closed = true;
			Params();
			AStart = (float)(AStart * Math.PI / 180);
			AEnd = (float)(AEnd * Math.PI / 180);
			N = (float)(Math.Round((AEnd -AStart)/Math.PI * numberOfParts));
			if(N < 3) N = 3;
			delta = (AEnd - AStart) / (N - 1);
			for(I = 0; I < N;I++) 
			{
				S = (float)Math.Sin(AStart);
				C = (float)Math.Cos(AStart);
				V = new DXFVertex();
				X = A * C;
				Y = B * S;
				V.point.X = point.X + X * ACos - Y * ASin;
				V.point.Y = point.Y + X * ASin + Y * ACos;
				V.point.Z = point.Z;
				entities.Add(V);
				AStart = AStart + delta;
			}
			nonZero = true;*/
			base.Loaded ();
        }
        public virtual void Params()
        {
            A = radius;
            B = radius;
            AStart = 0;
            AEnd = 360;
            ASin = 0;
            ACos = 1;
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
            if (FVisible)
                G.DrawEllipse(new Pen(RealColor, 1), P1.X, P1.Y, rd1 * 2, rd1 * 2);
        }
    }
}
