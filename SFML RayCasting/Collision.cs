using SFML.System;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting
{
    public class Collision
    {
        public Vector2f Pos;
        public AbsObject obj;
        public (Vector2f, Vector2f) NornalCollison;
        public bool IsGlass;
        public float Dist;
        public float LastDist;

        public Ray next = null;
        public Ray _this = null;
    }
}
