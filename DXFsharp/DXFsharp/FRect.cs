using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public struct FRect
    {
        public float left;
        public float top;
        public float z1;
        public float right;
        public float bottom;
        public float z2;
        public SFPoint topLeft;
        public SFPoint bottomRight;
    }
}
