using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML_RayCasting.Menedgers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Objects
{
    public class VertexObject : AbsObject
    {
        public VertexObject(string Name, Vector2f pos, string pathTexture, float SizeWall = 1, bool IsGlass = false, float zIndex = 1) :base(Name, pos, pathTexture, SizeWall, IsGlass, zIndex) 
        {
            Connections = new List<(Vector2f, Vector2f)> ();
        }
        public VertexObject(string Name, Vector2f pos, Color color, float SizeWall = 1, bool IsGlass = false, float zIndex = 1) : base(Name, pos, color, SizeWall, IsGlass, zIndex)
        {
            Connections = new List<(Vector2f, Vector2f)>();
        }
        public List<Vector2f> Points = new List<Vector2f>();
        public List<(Vector2f, Vector2f)> Connections { get; set; }
        public Dictionary<Vector2f, float> textureIndex = new Dictionary<Vector2f, float>();

        public override List<Collision> CheckColision(Ray ray, float zIndex)
        {
            List<Collision> collisions = new List<Collision>();
            if (MathF.Abs(zIndex - this.zIndex) > 5)
                return collisions;



            float minDist = float.MaxValue;
            // Пройти по всем соединениям и проверить их направление
            foreach (var connection in Connections)
            {

                // Проверить пересечение луча с текущим соединением
                Vector2f? intersectionPoint = MathUtils.RayIntersection(ray.StartPos, ray.Direction, connection.Item1, connection.Item2);

                // Проверка наличия пересечения
                if (intersectionPoint != null)
                {

                    float Dist = MathUtils.Distance(intersectionPoint.Value, ray.StartPos);
                    if(Dist > ray.MaxLen)
                        continue;

                    minDist = Dist;

                    Collision min = new Collision();
                    min._this = ray;
                    min.obj = this;
                    min.IsGlass = this.IsGlass;
                    min.Dist = Dist;
                    min.Pos = (Vector2f)intersectionPoint;
                    min.NornalCollison = connection;
                    collisions.Add(min);
                    ray.IsColision = true;
                }
            }
                return collisions;
            // Вернуть модифицированный луч с найденными коллизиями
        }
        public override void Draw()
        {
            foreach (var ray in Connections)
            {
                Vertex[] line = new Vertex[2];

                line[0] = new Vertex(ray.Item1, Color.Red);
                line[1] = new Vertex(ray.Item2, Color.Red);

                WindowMenedger.DrawVertex(line, PrimitiveType.Lines);
            }
        }
        public void AddRelativePoint(Vector2f relativePoint)
        {
            Vector2f absolutePoint = Position + relativePoint;
            Points.Add(absolutePoint);
        }
        public void AddConnection(int index1, int index2)
        {
            if (index1 >= 0 && index1 < Points.Count &&
                index2 >= 0 && index2 < Points.Count)
            {
                Connections.Add((Points[index1], Points[index2]));
                CreateIndexTexture(index1, index2);
            }
            else
            {
                throw new IndexOutOfRangeException("Point indices are out of range.");
            }
        }

        protected void CreateIndexTexture(int index1, int index2)
        {
            if (texture == null) return;

            var pos1 = Points[index1];
            var pos2 = Points[index2];

            float distance = MathUtils.Distance(pos1, pos2);

            if (textureIndex.Count == 0)
            {
                textureIndex.Add(pos1, 0);
            }

            float textureWidth = (float)texture.Size.X;
            float index = distance / distPyWidhtTexture; // Используем N единиц расстояния как одну ширину текстуры

            textureIndex.TryAdd(pos2, textureIndex.Last().Value + index);
        }


        public override Sprite GetSegment(Collision collision, float segmentWidth, float setUp, float setDown, float wallHeight)
        {

            Vector2f point = collision.Pos;
            (Vector2f, Vector2f) normal = collision.NornalCollison;

            float distance = MathUtils.Distance(normal.Item1, point);
            float index = distance / distPyWidhtTexture; // Используем 100 единиц расстояния как одну ширину текстуры
            float indexNormal = textureIndex[normal.Item1];

            index += indexNormal;

            while (index >= 1)
                index -= 1;

            Sprite sprite = new Sprite(texture);

            //setUp = (setUp / wallHeight) * texture.Size.Y;
            //setDown = (setDown / wallHeight) * texture.Size.Y;


			float xOnTexture = (float)texture.Size.X * index;

            float xStart = xOnTexture - segmentWidth / 2;
            float xEnd = xOnTexture + segmentWidth / 2;

            // Ensure the coordinates are within the texture boundaries
            if (xStart < 0) xStart = 0;
            if (xEnd > texture.Size.X) xEnd = texture.Size.X;


			// Create a rectangle for the texture part to be used in the sprite
			sprite.TextureRect = new IntRect((int)xStart, (int)setUp, ((int)xEnd - (int)xStart), ((int)texture.Size.Y - (int)setDown - (int)setUp));

            return sprite;
        }

		public bool IsPointInside(Vector2f point)
		{
			int crossings = 0;
			foreach (var connection in Connections)
			{
				Vector2f p1 = connection.Item1;
				Vector2f p2 = connection.Item2;

				// Проверяем пересечение горизонтального луча вправо от точки с ребром многоугольника
				if (((p1.Y <= point.Y && point.Y < p2.Y) || (p2.Y <= point.Y && point.Y < p1.Y)) &&
					(point.X < (p2.X - p1.X) * (point.Y - p1.Y) / (p2.Y - p1.Y) + p1.X))
				{
					crossings++;
				}
			}

			// Если количество пересечений нечетное, точка внутри многоугольника
			return (crossings % 2 == 1);
		}

	}
}
