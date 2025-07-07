using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TrackSystem.Types
{
    public class Vec3
    {
        public float x;
        public float y;
        public float z;

        public Vec3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
    }
}
