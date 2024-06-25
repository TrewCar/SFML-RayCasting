using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Menedgers
{
    public class MenedgerRays
    {
        public MenedgerRays(MapDef map, int maxRecurse, bool isNeedRecurce = false)
        {
            this.map = map;

            this.width = map.width;
            this.step = map.step; 
            this.POV = map.Pov;

            this.maxRecurse = maxRecurse;
            this.isNeedRecurce = isNeedRecurce;
        }
        public MenedgerRays(MapDef map)
        {
            this.map = map;
            this.Colisson = true;
            this.width = map.width;
            this.step = 2;
            this.POV = 360.0f;
            this.maxRecurse = 0;
            this.isNeedRecurce = false;
        }

        public int width;
        public int height;
        public float step;
        public float rayLength;
        public float POV;

        private bool Colisson;

        private MapDef map;
        private int maxRecurse;
        private bool isNeedRecurce;

        public List<Ray> rays { get; private set; } = new List<Ray>();

		public void CalcRay()
		{
			List<Ray> rays = new List<Ray>();
			int numSteps = (int)(this.width / this.step);
			var objs = map.Objects;

			for (int n = 0; n <= numSteps; n++)
			{
				int recursionDepth = 1;
				float relativeAngle = (this.POV / 2 * -1) + (this.POV / numSteps) * n;
				float angle = map.camera.Angle + relativeAngle;
				Vector2f direction = new Vector2f((float)Math.Cos(MathUtils.DegreesToRadians(angle)), (float)Math.Sin(MathUtils.DegreesToRadians(angle)));
				Ray rayRoot = Ray.CreateRay(map.camera.Position, direction, map.rayLength);
				Vector2f start = map.camera.Position;
				Ray ray = rayRoot;

				while (true)
				{
					foreach (var obj in objs)
					{
						List<Collision> collisions = obj.CheckColision(ray, map.camera.zIndex);
						ray.Colisions.AddRange(collisions);
					}

					ray.Colisions.Sort((v1, v2) => MathUtils.Distance(start, v1.Pos).CompareTo(MathUtils.Distance(start, v2.Pos)));

					if (ray.Colisions.Count > 0)
					{
						Collision closestCollision = ray.Colisions.First();

						if (closestCollision.IsGlass && recursionDepth < maxRecurse && isNeedRecurce)
						{
							recursionDepth++;
							Vector2f reflectionDirection = MathUtils.GetReflectedVector(ray.Direction, closestCollision.NornalCollison.Item2, closestCollision.NornalCollison.Item1);

							// Слегка сдвигаем начало нового луча, чтобы избежать зацикливания на той же нормали
							Vector2f newStartPos = MathUtils.StepToPoint(closestCollision.Pos, start, 0.001f);
							Ray reflectedRay = Ray.CreateRay(newStartPos, reflectionDirection, ray.MaxLen - MathUtils.Distance(start, closestCollision.Pos));

							closestCollision.next = reflectedRay;
							start = closestCollision.Pos;
							ray = reflectedRay;
						}
						else
						{
							break;
						}
					}
					else
					{
						break;
					}
				}

				rays.Add(rayRoot);
			}

			this.rays = rays;
		}



		public void SaveOnlyCollision()
        {
            List<Ray> rays = new List<Ray>();

            foreach(Ray ray in this.rays) { 
                if(ray.Colisions.Count > 0)
                {
                    rays.Add(ray);
                }
            }
            this.rays = rays;
        }

        public void DrawRays(PrimitiveType type)
        {
            foreach (var ray in rays)
            {
                List<Vertex> line = new List<Vertex>();
                if (Colisson)
                    line.Add(new Vertex(map.camera.Position, Color.Blue));
                else
                    line.Add(new Vertex(map.camera.Position, new Color(255, 255, 255)));


                Ray currentRay = ray;
                float brightness = 1.0f;
                while (currentRay != null)
                {
                    brightness *= 0.5f;
                    if (!currentRay.IsColision)
                    {

                        if (Colisson)
                            line.Add(new Vertex(currentRay.EndPoint, Color.Blue));
                        else
                            
                            line.Add(new Vertex(currentRay.EndPoint, new Color(255, 255, 255)));
                        break;
                    }

                    Collision colis = currentRay.Colisions.First();

                    Color fadedColor = new Color(
                        (byte)(255 * brightness),
                        (byte)(255 * brightness),
                        (byte)(255 * brightness),
                        255
                    );

                    if(Colisson)
                        line.Add(new Vertex(colis.Pos, Color.Blue));
                    else
                        line.Add(new Vertex(colis.Pos, fadedColor));

                    if (type == PrimitiveType.Lines)
                        currentRay = null;
                    else
                        currentRay = colis.next;
                }

                WindowMenedger.DrawVertex(line.ToArray(), type);
            }
        }
    }
}
