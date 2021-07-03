using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DXMiniEngine
{
    struct Vertex  
    {
        public Vertex(float x, float y, float z)
        {
            pos = new(x, y, z);
        }

        Vector3 pos;
    };
}
