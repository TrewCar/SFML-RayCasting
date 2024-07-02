using SFML.System;
using SFML_RayCasting.Maps;
using SFML_RayCasting.Menedgers;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Enemy
{
    internal class Banana : EnemyObject
    {
        public Banana(string Name, Vector2f pos, float zIndex = 1) : base(Name, pos, "Textures\\Banana.png", 1, false, zIndex)
        {
            this.AddRelativePoint(new Vector2f(0, 0));
            this.AddRelativePoint(new Vector2f(50, 0));
            this.Points.Reverse();
            this.AddConnection(0, 1);
            this.isCollision = false;
        }

        public override void Update(Vector2f pos, float deltaTime, MapDef map)
        {
            if (MathUtils.Distance(pos, this.Position) < 20)
                return;

            Position += MathUtils.DirectionFromPoitns(Position, pos) * (100 * deltaTime);

            base.Update(pos, deltaTime, map);
        }
    }
}
