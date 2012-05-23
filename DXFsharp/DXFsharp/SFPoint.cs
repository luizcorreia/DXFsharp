using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public struct SFPoint
    {
        public float X;
        public float Y;
        public float Z;
        public SFPoint(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
}
