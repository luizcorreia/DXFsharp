using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DXFsharp
{
    public class DXFEntity
    {
        public virtual bool AddEntity(DXFEntity E)
        {
            return false;
        }
        public DXFLayer layer;
        public CADImage Converter;
        public Color FColor = DXFConst.clByLayer;
        public bool Complex = false;
        protected bool FVisible = true;
        public virtual void Draw(System.Drawing.Graphics G) { }
        public virtual void Invoke(CADEntityProc Proc, CADIterate Params)
        {
            Proc(this);
        }
        public virtual void ReadEntities()
        {
            DXFEntity E;
            do
            {
                if (Converter.FValue == "EOF")
                {
                    return;
                }
                E = Converter.CreateEntity();
                if (E == null)
                {
                    Converter.Next();
                    break;
                }
                E.ReadState();
                if (E.GetType().IsSubclassOf(typeof(DXFEntity)))
                {
                    AddEntity(E);
                }
                Converter.Loads(E);
            }
            while (true);
        }
        public void ReadProps()
        {
            while (true)
            {
                Converter.Next();
                switch (Converter.FCode)
                {
                    case 0:
                        return;
                    //	case 66: 
                    //		Complex = Converter.FValue != "0";
                    //	break;

                    default:
                        ReadProperty();
                        break;
                }
            }
        }

        public virtual void ReadProperty() { }

        public void ReadState()
        {
            ReadProps();
            if (Complex)
            {
                ReadEntities();
            }
        }
        public virtual void Loaded() { }
    }
}
