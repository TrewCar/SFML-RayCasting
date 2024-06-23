using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Maps;
using SFML_RayCasting.Menedgers;
using SFML_RayCasting.Objects;

namespace SFML_RayCasting
{
    public static class RenderWindowView
    {
        public static void Render(MapDef map, List<Ray> rays)
        {
            float middleHeight = WindowMenedger.Height / 2.0f;
            float stepWidth = WindowMenedger.Widht / rays.Count;

            for (int i = 0; i < rays.Count; i++)
            {
                if (rays[i].Colisions.Count > 0)
                {
                    for (int j = rays[i].Colisions.Count - 1; j > 0; j--)
                    {
                        Collision colis = rays[i].Colisions[j];

                        DrawCollision(colis, i, map.camera.Angle - rays[i].Angle, middleHeight, stepWidth);
                    }
                    DrawCollision(rays[i].Colisions.First(), i, map.camera.Angle - rays[i].Angle, middleHeight, stepWidth);
                }
            }
        }
        private static void DrawCollision(Collision colis, int i, float rAngle, float middleHeight, float stepWidth)
        {
            AbsObject obj = colis.obj;
            float dist = colis.Dist;
            Sprite sp = obj.GetSegment(colis, stepWidth);

            // Коррекция дистанции для устранения эффекта рыбьего глаза
            float angleDifference = MathUtils.DegreesToRadians(rAngle);
            float correctedDist = dist * (float)Math.Cos(angleDifference);

            // Перспективное сокращение высоты стены

            float defWallHeight =  WindowMenedger.Height / correctedDist * 50;

            float wallHeight = WindowMenedger.Height / correctedDist * 50 * obj.SizeWall;

            float middle = wallHeight - defWallHeight;

            float upHeight = middleHeight - wallHeight / 2.0f - middle/2;
            float downHeight = middleHeight + wallHeight / 2.0f + middle / 2;

            if (sp != null)
            {
                sp.Position = new Vector2f(stepWidth * i, upHeight);

                sp.Scale = new Vector2f(stepWidth / sp.TextureRect.Width, wallHeight / sp.TextureRect.Height);

                WindowMenedger.window.Draw(sp);
                return;
            }

            Color color = obj.Color;

            Vertex[] vertices =
            {
                new Vertex(new Vector2f(stepWidth * i, upHeight), color),
                new Vertex(new Vector2f(stepWidth * i + stepWidth, upHeight), color),
                new Vertex(new Vector2f(stepWidth * i + stepWidth, downHeight), color),
                new Vertex(new Vector2f(stepWidth * i, downHeight), color)
            };

            WindowMenedger.DrawVertex(vertices, PrimitiveType.Quads);
        }



    }
}
