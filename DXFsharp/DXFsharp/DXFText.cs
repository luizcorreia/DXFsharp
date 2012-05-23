using System.Drawing;

namespace DXFsharp
{
    public class DXFText : DXFCustomVertex
    {
        public string fontName = "";
        public bool backward;
        public FRect box;
        public Font font;
        public Color fontColor;
        public byte hAlign;
        public float height;
        public DXFMText mText;
        public float obliqueAngle;
        public SFPoint point1;
        public float rotation;
        public float scale;
        public SFPoint extrusion;
        public SFPoint startPoint;
        //public DXFStyle style;
        public string text = "";
        public bool upsideDown;
        public byte vAlign;
        public bool winFont;


        public void SetTextStr(string Value)
        {
            int vPos, I;
            text = Value;
            if (DXFConst.macroStrings != null)
            {
                for (I = 0; I < DXFConst.macroStrings.Count; I++)
                {
                    vPos = ((string)DXFConst.macroStrings[I]).IndexOf("=");
                    if (vPos == 0) continue;
                    text.Replace((string)DXFConst.macroStrings[I],
                        ((string)DXFConst.macroStrings[I]).Substring(vPos + 1, ((string)DXFConst.macroStrings[I]).Length));
                }
            }
            text = text.Replace("%%d", "°");
            text = text.Replace("%%p", "±");
            text = text.Replace("%%u", @"\L");
            text = text.Replace("%%127", "?");
            text = text.Replace("%%128", "ˆ");
            text = text.Replace("%%176", "°");
            text = text.Replace("%%179", "?");
            text = text.Replace("%%c", @"\U+2205");
            DXFConst.ReplaceNToDiameter(text);
        }

        /*public void SetStyle(DXFStyle AStyle)
        {
            int vPos;
            style = AStyle;
            font.Name = style.fontName;
            vPos = style.fontNamePos.IndexOf(".");
            if(vPos != 0) font.Name = style.fontName.Substring(1, vPos - 1); 
        }*/

        /*public void SetOblique(float Value)
        {
            obliqueAngle = Value;
            if(Math.Abs(Value) < 10) font = new Font("Times New Roman", 10); //temp
                else font = new Font("Times New Roman", 10, FontStyle.Italic); //temp
        }*/

        public override void ReadProperty()
        {
            int vFlag;
            switch (Converter.FCode)
            {
                case 1:
                    SetTextStr(Converter.FValue);
                    break;
                case 7:
                    //SetStyle(Converter.StyleByName(Converter.FValue));
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
                    scale = Converter.FloatValue();
                    break;
                case 50:
                    rotation = Converter.FloatValue();
                    break;
                case 51:
                    //SetOblique(Converter.FloatValue());
                    break;
                case 71:
                    vFlag = Converter.IntValue();
                    backward = (vFlag & 2) > 0;
                    upsideDown = (vFlag & 4) > 0;
                    break;
                case 72:
                    hAlign = Converter.ByteValue();
                    break;
                case 73:
                    vAlign = Converter.ByteValue();
                    break;
                case 210:
                    extrusion.X = Converter.FloatValue();
                    break;
                case 220:
                    extrusion.Y = Converter.FloatValue();
                    break;
                case 230:
                    extrusion.Z = Converter.FloatValue();
                    break;
                default:
                    base.ReadProperty();
                    break;
            }
        }

        public override void Invoke(CADEntityProc Proc, CADIterate Params)
        {
            if (mText == null) Proc(this);
            else
                mText.Invoke(Proc, Converter.FParams);
        }

        public override void Loaded()
        {
            /*int P, C;
            if(style == null) SetStyle(Converter.StyleByName(sTextStyleStandardName));
            if(backward && upsideDown) 
                if(rotation > 180) rotation = rotation - 180;
                 else rotation = rotation + 180;
            Converter.ScanOEM(text);
            if(Extruded(extrusion) || (text.IndexOf(@"\U+2205") != 0) || (text.IndexOf(@"\L") != 0)) 
            {
                mText = new DXFMText();
                mText.point = point;
                mText.extrusion = extrusion;
                mText.setAngle(rotation);
                mText.color = fontColor;
                mText.height = height;
                mText.align = 10;// 7 in previous version
                mText.text = text;
                mText.layer = layer;
                mText.paperSpace = paperSpace;
                mText.Loaded();
                fBox = mText.box;
                return;
            }
            while(true) 
            {
                P = text.IndexOf(@"\U+");
                if((P == 0) || (P > text.Length - 6)) break;
                C = StrToIntDef('$' + Copy(Text, P + 5, 2), Ord('?'));
                Delete(FText, P, 6);
                FText[P] = Chr(C);
            }
            if(! HasSecond) {
                hAlign = 0;
                vAlign = 0;
            }
            DoNewText(text);*/
            if (layer == null) layer = Converter.LayerByName("0");
        }

        public override void Draw(Graphics G)
        {
            SFPoint P1;
            Color RealColor = DXFConst.EntColor(this, Converter.FParams.Insert);
            if (RealColor == DXFConst.clNone) RealColor = Color.Black;
            P1 = this.Converter.GetPoint(point);
            float h1 = height * Converter.FScale;
            P1.Y = P1.Y - h1;
            SolidBrush br1 = new SolidBrush(RealColor);
            Font f1 = new Font("Times New Roman", h1);
            if (FVisible)
            {
                /*GraphicsContainer c1 = G.BeginContainer();
                if(rotation != 0) 
                {
                    string str = string.Empty;
                    SizeF textSize = SizeF.Empty;
                    PointF textLocation = new PointF(P1.X, P1.Y); //PointF.Empty;
                    StringFormat strfmt = new StringFormat();
                    str = text;
                    //strfmt.FormatFlags = StringFormatFlags.DirectionVertical;
                    Size ClientSize = new Size((int) G.VisibleClipBounds.Width, (int) G.VisibleClipBounds.Height); 
                    textSize = G.MeasureString(str, f1, ClientSize, strfmt);
                    textLocation = new Point(ClientSize.Width / 2, ClientSize.Height / 4);
                    RectangleF rectFSrc = new RectangleF(PointF.Empty, textSize);
                    RectangleF rectFDest = new RectangleF(new PointF(textLocation.X + textSize.Width, textLocation.Y + textSize.Height / 2), textSize);
                    GraphicsContainer container = G.BeginContainer(rectFDest, rectFSrc, GraphicsUnit.Pixel);
                    G.RotateTransform(rotation);
                    G.DrawString(str, f1, br1, PointF.Empty, strfmt);
                    strfmt.Dispose();
                } else G.DrawString(text, f1, br1, P1.X, P1.Y);
                G.EndContainer(c1); */
                if (rotation == 90) G.DrawString(text, new Font("Times New Roman", h1),
                                                 br1, P1.X - h1, P1.Y - (text.Length * h1 / 1.6f),
                                                 new StringFormat(StringFormatFlags.DirectionVertical));
                else
                    G.DrawString(text, new Font("Times New Roman", h1), br1, P1.X, P1.Y);
            }
        }
    }
}
