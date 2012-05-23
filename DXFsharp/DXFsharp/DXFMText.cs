using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public class DXFMText : DXFInsert
    {
        public string addText;
        public byte align;
        public float height;
        public SFPoint point1;
        public float rectHeight;
        public float rectWidth;
        public string text;

        public DXFMText()
        {
            Scale = new SFPoint(1.0f, 1.0f, 1.0f);
            FColor = DXFConst.clByLayer;
            Matrix.IdentityMat();
        }

        private void AdjustChildren()
        {
            int I, K = 0;
            DXFText T;
            float X = 0, Y = 0;
            for (I = 0; I < FBlock.Entities.Count; I++)
            {
                T = (DXFText)FBlock.Entities[I];
                if (T.box.top != Y)
                {
                    Row(K, I, X);
                    K = I;
                }
                X = T.box.right;
                Y = T.box.top;
            }
            Row(FBlock.Entities.Count - 1, FBlock.Entities.Count, X);
        }

        private void Row(int AStart, int AEnd, float X)
        {
            int I;
            DXFText T;
            X = rectWidth - X;
            if (X < 0) return;
            if ((align == 2) || (align == 5) || (align == 8)) X = X / 2;
            if (AStart < 0) // added me
                AStart = 0;
            for (I = AStart; I < AEnd; I++)
            {
                T = (DXFText)FBlock.Entities[I];
                T.startPoint.X = T.startPoint.X + X;
                DXFConst.OffsetFRect(T.box, X, 0, 0);
            }
        }

        public override void ReadProperty()
        {
            switch (Converter.FCode)
            {
                case 2:
                case 42:
                case 43:
                case 72:
                case 73:
                    return;
                case 1:
                    text = addText + Converter.FValue;
                    break;
                case 3:
                    addText = addText + Converter.FValue;
                    break;
                case 11:
                    point1.X = Converter.FloatValue();
                    break;
                case 21:
                    point1.Y = Converter.FloatValue();
                    break;
                case 31:
                    point1.Z = Converter.FloatValue();
                    break;
                case 40:
                    height = Converter.FloatValue();
                    break;
                case 41:
                    rectWidth = Converter.FloatValue();
                    break;
                case 44:
                    rectHeight = Converter.FloatValue();
                    break;
                case 71:
                    align = (byte)Converter.FloatValue();
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }

        private void ReplaceNToDiameter(string S)
        {
            if (S.CompareTo('n') == 0) S = S.Replace("n", @"\U+2205");
        }


        public override void Loaded()
        {
            /*string S;
            int P = 0;
            DXFText T, T0;
            double X, Y;
            if((point1.X != DXFConst.Illegal) && (point1.Y != DXFConst.Illegal)) 
                SetAngle(Math.Atan2(point1.Y, point1.X) * 180 / Math.PI);
            S = text;
            T0 = null;
            X = 0;
            Y = 0;
            S = S.Replace("{", "");
            S = S.Replace("}", "");
            ReplaceNToDiameter(S);
            while(S != "") 
            {
                T = new DXFText();
                FBlock.AddEntity(T);
                T.SetTextStr(S);
                T.layer = layer;
                T.height = height;
                T.vAlign = 3;
                T.point = new SFPoint((float)X, (float)Y, 0);
                T.point1 = T.point;
                T.fontColor = color;
                //T.Parse(S, T0);
                T.point1 = T.point;
                //T.paperSpace = paperSpace;
                T.Loaded();
                if((rectWidth > 0) && (T.box.Right > rectWidth)) 
                {
                    //P = Pos(" ", T.text);
                    if(P == T.text.Length) P = 0;
                    if((P > 0) || (T.box.Left > 0))
                    {
                        S.Insert(1, T.text);
                        //FBlock.Entities.Count = FBlock.Entities.Count - 1;
                        if(P == 0) S = S.Insert(1,@"\P");
                         else S = S.Insert(P + 1, @"\ ");
                        continue;
                    }
                }
                X = T.box.Right;
                Y = T.box.Top;
                T0 = T;
            }
            if((rectWidth > 0) && ((align != 0)&&(align != 1)&&
                (align != 4)&&(align != 7))) AdjustChildren();
            //FBlock.loaded = false;
            FBlock.Loaded();
            //FBlock.inserts = 0;
            //X = FBlock.box.Right - FBlock.FBox.Left;
            //Y = FBlock.box.Top - FBlock.FBox.Bottom;
            if(X < rectWidth) X = rectWidth;
            switch(align) 
            {
                case 2:
                case 5:
                case 8: 
                    //FBlock.offset.X = X / 2;
                    break;
                case 3:
                case 6: 
                case 9:
                    //FBlock.offset.X = X;
                    break;
                case 10:	// new
                    //FBlock.offset.X = 0;
                    break;
            }
            switch(align) 
            {
                case 4:
                case 5:
                case 6:
                    //FBlock.offset.Y = -Y / 2;
                    break;
                case 7:
                case 8:
                case 9: 
                    //FBlock.offset.Y = -Y;
                    break;
                case 10:	// new
                    //FBlock.offset.Y = -Y / 2 - Height / 2;
                    break;
            }
            */
            base.Loaded();
        }
    }
}
