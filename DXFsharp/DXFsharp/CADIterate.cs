using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public delegate void CADEntityProc(DXFEntity Ent);
    public struct CADIterate
    {
        public SFPoint Scale;
        public DXFMatrix matrix;
        public DXFInsert Insert;
    }
}
