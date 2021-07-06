using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DXMiniEngine
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex  
    {
        public Vertex(float x, float y, float z)
        {
            pos = new(x, y, z);
        }

        Vector3 pos;
    };
}
