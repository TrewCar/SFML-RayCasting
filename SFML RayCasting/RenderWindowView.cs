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
				float dist = MathUtils.Distance(rays[i].StartPos, rays[i].EndPoint);
				DrawSky(dist, i, map.camera.Angle - rays[i].Angle, middleHeight, stepWidth, map.camera.zIndex);
				if (rays[i].Colisions.Count > 0)
				{
					for (int j = rays[i].Colisions.Count - 1; j > 0; j--)
					{
						Collision colis = rays[i].Colisions[j];

						DrawCollision(colis, rays[i], i, map.camera.Angle - rays[i].Angle, middleHeight, stepWidth, map.camera.zIndex);
					}
					DrawCollision(rays[i].Colisions.First(), rays[i], i, map.camera.Angle - rays[i].Angle, middleHeight, stepWidth, map.camera.zIndex, true);
				}

			}
		}
		private static void DrawCollision(Collision colis, Ray ray, int i, float rAngle, float middleHeight, float stepWidth, float zIndexPos, bool glass = false)
		{
			AbsObject obj = colis.obj;
			float dist = colis.Dist;
			Sprite sp = obj.GetSegment(colis, stepWidth);

			float brightness = Math.Max(0, 1 - dist / Program.maxDistRay);
			byte colorValue = (byte)(255 * brightness);
			sp.Color = new Color(colorValue, colorValue, colorValue);

			// Коррекция дистанции для устранения эффекта рыбьего глаза
			float angleDifference = MathUtils.DegreesToRadians(rAngle);
			float correctedDist = dist * (float)Math.Cos(angleDifference);

			float zIndex = (obj.zIndex - zIndexPos);

			// Перспективное сокращение высоты стены
			float defWallHeight = WindowMenedger.Height / correctedDist * 50;
			float wallHeight = WindowMenedger.Height / correctedDist * 50 * obj.SizeWall;

			float temp = wallHeight;
			wallHeight *= zIndex;

			temp = wallHeight - temp;

			float middle = wallHeight - defWallHeight;
			wallHeight -= temp;
			float upHeight = (middleHeight - wallHeight / 2.0f - middle / 2);
			float downHeight = (middleHeight + wallHeight / 2.0f + middle / 2);

			if (sp != null)
			{
				sp.Position = new Vector2f(stepWidth * i, upHeight);

				sp.Scale = new Vector2f(stepWidth / sp.TextureRect.Width, wallHeight / sp.TextureRect.Height);

				WindowMenedger.window.Draw(sp);
				if (colis.IsGlass && colis.next != null && glass)
				{
					try
					{
						if (colis.next.Colisions.Count == 0)
						{
							float dis2t = MathUtils.Distance(ray.StartPos, ray.EndPoint);
							DrawSky(colis.LastDist, i, rAngle, middleHeight, stepWidth, zIndexPos);
							return;
						}
						colis.next.Colisions.Sort((v1, v2) => MathUtils.Distance(colis.next.StartPos, v2.Pos).CompareTo(MathUtils.Distance(colis.next.StartPos, v1.Pos)));
						for (int j = 0; j < colis.next.Colisions.Count; j++)
						{

							colis.next.Colisions[j].Dist += colis.Dist;
							DrawCollision(colis.next.Colisions[j], ray, i, rAngle, middleHeight, stepWidth, zIndexPos, glass);
						}
						//Collision coliss = colis.next.Colisions.First() ;

						//               coliss.Dist += colis.Dist;
						//               DrawCollision(coliss, ray, i, rAngle, middleHeight, stepWidth, zIndexPos, glass);

					}
					catch
					{
						float dis2t = MathUtils.Distance(ray.StartPos, ray.EndPoint);
						DrawSky(dist, i, rAngle, middleHeight, stepWidth, zIndexPos);

					}
				}
				return;
			}
			/*
            Color color = new Color(15,15,15,255);

            Vertex[] vertices =
            {
                new Vertex(new Vector2f(stepWidth * i, upHeight), color),
                new Vertex(new Vector2f(stepWidth * i + stepWidth, upHeight), color),
                new Vertex(new Vector2f(stepWidth * i + stepWidth, downHeight), color),
                new Vertex(new Vector2f(stepWidth * i, downHeight), color)
            };

            WindowMenedger.DrawVertex(vertices, PrimitiveType.Quads);
			if (colis.IsGlass && colis.next != null && glass)
			{
				try
				{
                    if(colis.next.Colisions.Count == 0)
                    {
						float dis2t = MathUtils.Distance(ray.StartPos, ray.EndPoint);
						DrawSky(colis.LastDist, i, rAngle, middleHeight, stepWidth, zIndexPos);
                        return;
					}
					Collision coliss = colis.next.Colisions.First() ;

                    coliss.Dist += colis.Dist;
                    DrawCollision(coliss, ray, i, rAngle, middleHeight, stepWidth, zIndexPos, glass);
                    
				}
				catch {
					float dis2t = MathUtils.Distance(ray.StartPos, ray.EndPoint);
                    DrawSky(dist, i, rAngle, middleHeight, stepWidth, zIndexPos);

				}
			}
            */
		}
		private static void DrawSky(float dist, int i, float rAngle, float middleHeight, float stepWidth, float zIndexPos)
		{


			// Коррекция дистанции для устранения эффекта рыбьего глаза
			float angleDifference = MathUtils.DegreesToRadians(rAngle);
			float correctedDist = dist * (float)Math.Cos(angleDifference);



			// Перспективное сокращение высоты стены
			float defWallHeight = WindowMenedger.Height / correctedDist * 50;
			float wallHeight = WindowMenedger.Height / correctedDist * 50;


			float middle = wallHeight - defWallHeight;
			float upHeight = (middleHeight - wallHeight / 2.0f - middle / 2);
			float downHeight = (middleHeight + wallHeight / 2.0f + middle / 2);

			Color color = new Color(255, 255, 255, 255);

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
