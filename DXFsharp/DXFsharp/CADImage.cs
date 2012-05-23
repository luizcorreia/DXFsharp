using System;
using System.Drawing;
using System.IO;
using System.Globalization;

namespace DXFsharp
{
    public class CADImage
    {
        public CADImage()
        {
            N.NumberDecimalSeparator = ".";
            FParams.Scale.X = 1;
            FParams.Scale.Y = 1;
            FParams.Scale.Z = 1;
        }
        public Point Base;
        public DXFSection FBlocks;
        public static Graphics FGraphics;
        public int FCode;
        public DXFSection FEntities;
        public CADIterate FParams = new CADIterate();
        protected StreamReader FStream;
        public DXFSection FMain;
        public string FValue;
        public NumberFormatInfo N = new NumberFormatInfo();
        public float FScale = 1;
        public DXFTable layers;

        public DXFLayer LayerByName(string AName)
        {
            DXFLayer Result = null;
            int I;
            if (layers == null) layers = new DXFTable();
            for (I = 0; I < layers.Entities.Count; I++)
            {
                if (AName.ToLower() == ((DXFLayer)layers.Entities[I]).name.ToLower())
                    Result = ((DXFLayer)layers.Entities[I]);
            }
            if (Result == null)
            {
                Result = new DXFLayer();
                Result.name = AName;
                layers.AddEntity(Result);
            }
            return Result;
        }
        public void Draw(Graphics e)
        {
            if (FMain == null)
                return;
            FGraphics = e;
            FEntities.Iterate(new CADEntityProc(DrawEntity), FParams);
        }
        protected static void DrawEntity(DXFEntity Ent)
        {
            Ent.Draw(FGraphics);
        }
        public DXFBlock FindBlock(string Name)
        {
            DXFBlock vB = null;
            foreach (DXFBlock vBlock in FBlocks.Entities)
            {
                if (vBlock.Name == Name)
                {
                    vB = vBlock;
                }
            }
            return vB;

        }
        public void Iterate(CADEntityProc Proc, CADIterate Params)
        {
            FParams = Params;
            FEntities.Iterate(Proc, Params);
        }
        public float FloatValue()
        {
            float F;
            F = Convert.ToSingle(FValue, N);
            return F;
        }
        public int IntValue()
        {
            int F;
            F = Convert.ToInt32(FValue, N);
            return F;
        }
        public byte ByteValue()
        {
            byte F;
            F = Convert.ToByte(FValue, N);
            return F;
        }
        public void LoadFromFile(string FileName)
        {
            FMain = new DXFSection();
            FMain.Converter = this;
            if (FStream == null)
            {
                FStream = new StreamReader(FileName);
            }
            FMain.Complex = true;
            FMain.ReadState();
        }

        public SFPoint GetPoint(SFPoint Point)
        {
            SFPoint P;
            if (FParams.matrix != null)
            {
                P.X = Base.X + FScale * (Point.X * FParams.Scale.X + FParams.matrix.data[3, 0]);
                P.Y = Base.Y - FScale * (Point.Y * FParams.Scale.Y + FParams.matrix.data[3, 1]);
            }
            else
            {
                P.X = Base.X + FScale * (Point.X * FParams.Scale.X);
                P.Y = Base.Y - FScale * (Point.Y * FParams.Scale.Y);
            }
            P.Z = Point.Z * FScale;
            return P;
        }
        public DXFEntity CreateEntity()
        {
            DXFEntity E;
            switch (FValue)
            {
                case "ENDSEC":
                    return null;
                case "ENDBLK":
                    return null;
                case "ENDTAB":
                    return null;
                case "LINE":
                    E = new DXFLine();
                    break;
                case "SECTION":
                    E = new DXFSection();
                    break;
                case "BLOCK":
                    E = new DXFBlock();
                    break;
                case "INSERT":
                    E = new DXFInsert();
                    break;
                case "TABLE":
                    E = new DXFTable();
                    break;
                case "CIRCLE":
                    E = new DXFCircle();
                    break;
                case "LAYER":
                    E = new DXFLayer();
                    break;
                case "TEXT":
                    E = new DXFText();
                    break;
                case "MTEXT":
                    E = new DXFMText();
                    break;
                case "ARC":
                    E = new DXFArc();
                    break;
                case "ELLIPSE":
                    E = new DXFEllipse();
                    break;
                default:
                    E = new DXFEntity();
                    break;
            }
            E.Converter = this;
            return E;
        }
        public void Next()
        {
            FCode = Convert.ToInt32(FStream.ReadLine());
            FValue = FStream.ReadLine();
        }
        public static Color IntToColor(int Value)
        {
            Color[] First = new Color[] {DXFConst.clByBlock, Color.Red, Color.Yellow, 
											Color.Lime, Color.Aqua, Color.Blue, Color.Fuchsia, 
											DXFConst.clNone, Color.Gray, Color.Silver};
            Color[] Last = new Color[] {DXFConst.clByBlock, Color.FromName("" + 0x282828), 
										   Color.FromName("" + 0x505050), Color.FromName("" + 0x787878), 
										   Color.FromName("" + 0xA0A0A0), Color.White};
            int V, H, L, S, Result;
            Result = Value;
            if (Result < 0) return First[7];
            V = Result & 255;
            if (V < 10) return First[V];
            else
            {
                if (V >= 250) return Last[V - 250];
                else
                {
                    H = (int)(V / 10) - 1;
                    L = V % 10;
                    S = L & 1;
                    L = 5 - (L >> 1);
                    Result = (RGB(H, S, L) << 16) + (RGB(H + 8, S, L) << 8) + RGB(H + 16, S, L);
                    if (Result != 0) Result = Result | 0x2000000;
                }
            }
            byte R, G, B;
            R = (byte)(Result >> 32);
            G = (byte)(Result >> 8);
            B = (byte)(Result >> 16);
            return Color.FromArgb(R, G, B);
        }
        private static byte RGB(int Index, int S, int L)
        {
            byte[] Pal = new byte[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 51, 102, 204, 255, 255, 255, 
										255, 255, 255, 255, 255, 255, 204, 102, 51};
            int Result;
            if (Index > 23) Index -= 24;
            Result = Pal[Index];
            if ((S != 0) && (Result < 204)) Result += 102;
            Result *= L;
            Result /= 5;
            return (byte)Result;
        }
        public void Loads(DXFEntity E)
        {
            E.Loaded();
        }
    }
}
