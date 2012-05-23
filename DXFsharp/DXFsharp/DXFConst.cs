using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;


namespace DXFsharp
{
    public class DXFConst
    {
        public static readonly int byBlock = 0x3FFFFFFF;
        public static readonly int byLayer = 0x2FFFFFFF;
        public static readonly int none = 0x1FFFFFFF;
        public static readonly Color clByBlock = Color.FromName("" + DXFConst.byBlock);
        public static readonly Color clByLayer = Color.FromName("" + DXFConst.byLayer);
        public static readonly Color clNone = Color.FromName("" + DXFConst.none);
        public static readonly double Illegal = -0x5555 * 65536.0 * 65536.0;
        public static readonly float accuracy = 0.000001f;

        public static ArrayList macroStrings = new ArrayList();

        public static Color EntColor(DXFEntity E, DXFInsert Ins)
        {
            DXFInsert vIns = Ins;
            DXFEntity Ent = E;
            Color Result = DXFConst.clNone;
            if (Ent is DXFVisibleEntity) Result = E.FColor;
            /*if(Ent is Polyline) 
                Result = ((Polyline)Ent).Pen.Pen.Color;*/
            if (E.layer == null) return Result;
            if ((Result == clByLayer) || (Result == clByBlock))
            {
                if ((vIns == null) || ((Result == clByLayer) && (Ent.layer.name != "0")))
                {
                    if (Result == clByLayer)
                    {
                        if (Ent.layer.color != clNone)
                            Result = Ent.layer.color;
                        else Result = Color.Black;
                    }
                }
                else
                {
                    while (vIns != null)
                    {
                        Result = vIns.color;
                        if ((Result != clByBlock) && !((Result == clByLayer) &&
                            (vIns.layer.name == "0")))
                        {
                            if (Result == clByLayer)
                                Result = vIns.layer.color;
                            break;
                        }
                        if ((vIns.owner == null) && (Result == clByLayer))
                            Result = vIns.layer.color;
                        vIns = vIns.owner;
                    }
                }
            }
            if ((Result == clByLayer) || (Result == clByBlock))
                Result = clNone;
            return Result;
        }

        public static void OffsetFRect(FRect R, float DX, float DY, float DZ)
        {
            R.left = R.left + DX;
            R.top = R.top + DY;
            R.right = R.right + DX;
            R.bottom = R.bottom + DY;
            R.z1 = R.z1 + DZ;
            R.z2 = R.z2 + DZ;
        }

        public static double Radian(double Angle)
        {
            return (Angle * Math.PI / 180);
        }

        public static void ReplaceNToDiameter(string S)
        {
            if (S.IndexOf("n") == 0) S = S.Replace("n", @"\U+2205");
        }
    }
}
