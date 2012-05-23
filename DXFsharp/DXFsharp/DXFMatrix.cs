using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFsharp
{
    public class DXFMatrix
    {
        public float[,] data = new float[4, 3];
        public void IdentityMat()
        {
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;
        }
        public static DXFMatrix MatXMat(DXFMatrix A, DXFMatrix B)
        {
            int I, J;
            DXFMatrix Result = new DXFMatrix();
            for (I = 0; I < 4; I++)
            {
                for (J = 0; J < 3; J++)
                    Result.data[I, J] = A.data[I, 0] * B.data[0, J] + A.data[I, 1] * B.data[1, J] +
                        A.data[I, 2] * B.data[2, J];
            }
            for (J = 0; J < 3; J++)
                Result.data[3, J] = Result.data[3, J] + B.data[3, J];
            return Result;
        }
        public SFPoint PtXMat(SFPoint P)
        {
            SFPoint Result = new SFPoint();
            Result.X = Part(0, P);
            Result.Y = Part(1, P);
            Result.Z = Part(2, P);
            return Result;
        }
        private float Part(int I, SFPoint P)
        {
            return (P.X * data[0, I] + P.Y * data[1, I] + P.Z * data[2, I] + data[3, I]);
        }
        public static DXFMatrix StdMat(SFPoint S, SFPoint P)
        {
            DXFMatrix Result = new DXFMatrix();
            Result.data[0, 0] = S.X;
            Result.data[1, 1] = S.Y;
            Result.data[2, 2] = S.Z;
            DXFMatrix.MatOffset(Result, P);
            return Result;
        }
        private static void MatOffset(DXFMatrix M, SFPoint P)
        {
            M.data[3, 0] = P.X;
            M.data[3, 1] = P.Y;
            M.data[3, 2] = P.Z;
        }
    }
}
