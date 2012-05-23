using System;
using System.Drawing;

namespace DXFsharp
{
    public class DXFInsert : DXFVisibleEntity
    {
        private DXFMatrix matrix = new DXFMatrix();
        public DXFInsert owner;
        public DXFBlock FBlock;
        public SFPoint Scale;
        public Color color;
        public float angle; //sm
        public double sin; //sm
        public double cos; //sm

        public DXFMatrix Matrix
        {
            get
            {
                return matrix;
            }
            set
            {
                matrix = value;
            }
        }

        public DXFInsert()
        {
            Scale.X = 1;
            Scale.Y = 1;
            Matrix.IdentityMat();
        }

        public void SetAngle(double Value)
        {
            angle = (float)Value;
            sin = Math.Sin(DXFConst.Radian(Value));
            cos = Math.Cos(DXFConst.Radian(Value));
        }

        public override void Invoke(CADEntityProc Proc, CADIterate Params)
        {
            if (Params.matrix == null) Params.matrix = new DXFMatrix();
            if (FBlock == null) return;
            CADIterate Iter;
            Iter = Params;
            Params.matrix = matrix;
            Params.Scale = Scale;
            Params.Insert = this;
            Converter.FParams = Params;
            FBlock.Iterate(Proc, Params);
            Converter.FParams = Iter;
            Params = Iter;
            owner = Params.Insert;
        }
        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 2:
                    FBlock = Converter.FindBlock(Converter.FValue);
                    break;
                case 41:
                    Scale.X = Converter.FloatValue();
                    break;
                case 42:
                    Scale.Y = Converter.FloatValue();
                    break;
                case 62:
                    color = CADImage.IntToColor(Convert.ToInt32(Converter.FValue, Converter.N));
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }
        public override void Loaded()
        {
            matrix = new DXFMatrix();
            matrix = DXFMatrix.MatXMat(matrix, DXFMatrix.StdMat(new SFPoint(1, 1, 1), Point1));
        }
        /*public override bool AddEntity(DXFEntity E)
        {
            bool Result;
            Result = (E is DXFAttrib);
            if(Result) FAttribs.Add(E);
            return Result;
        }*/
    }
}
