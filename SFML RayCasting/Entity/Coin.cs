using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Maps;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Entity
{
    public class Coin : VertexObject
    {
        public Coin(string Name, Vector2f pos, float zIndex = 1.3f) : base(Name, pos, Path.Combine("Textures", "gold.png"), 0.1f, false, zIndex)
        {
            baseZ = zIndex;

            InstantCircule(6, 5);
        }

        private float rotationAngle = 0f;
        private float floatTime = 0f;

        private float baseZ;

        public override void Update(Vector2f pos, float deltaTime, MapDef map)
        {

            floatTime += deltaTime;
            this.zIndex = baseZ + MathF.Sin(floatTime * 2f) * 0.3f;

            rotationAngle += 120f * deltaTime; // скорость вращения (градусы в секунду)
            RotatePoints(deltaTime);

            base.Update(pos, deltaTime, map);
        }

        private void RotatePoints(float deltaTime)
        {
            float radians = rotationAngle * (MathF.PI / 180f) * deltaTime / 2;

            for (int i = 0; i < Points.Count; i++)
            {
                Vector2f dir = Points[i] - Position;

                float rotatedX = dir.X * MathF.Cos(radians) - dir.Y * MathF.Sin(radians);
                float rotatedY = dir.X * MathF.Sin(radians) + dir.Y * MathF.Cos(radians);

                Points[i] = new Vector2f(rotatedX, rotatedY) + Position;
            }

            Connections.Clear();
            for (int i = 0; i < Points.Count; i++)
            {
                int next = (i + 1) % Points.Count;
                Connections.Add((Points[i], Points[next]));
            }

            RebuildTextureIndex();
        }
    }
}
