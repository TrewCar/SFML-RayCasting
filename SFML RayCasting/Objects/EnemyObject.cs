using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Objects
{
    public class EnemyObject : VertexObject
    {
        public EnemyObject(string Name, Vector2f pos, string pathTexture, float SizeWall = 1, bool IsGlass = false, float zIndex = 1) : base(Name, pos, pathTexture, SizeWall, IsGlass, zIndex)
        {
        }

        public EnemyObject(string Name, Vector2f pos, Color color, float SizeWall = 1, bool IsGlass = false, float zIndex = 1) : base(Name, pos, color, SizeWall, IsGlass, zIndex)
        {
        }
        
        protected float radius => MathUtils.Distance(Connections[0].Item1, Connections[0].Item2) / 2;
        protected (Vector2f, Vector2f) FindPoints(Vector2f C, Vector2f D, float radius)
        {
            // Вектор CD
            Vector2f CD = D - C;
            // Нормализуем вектор CD
            Vector2f CD_normalized = MathUtils.Normalized(CD);
            // Находим перпендикулярный вектор к CD
            Vector2f perpendicular = new Vector2f(-CD_normalized.Y, CD_normalized.X);
            // Найдем точки A и B, которые лежат на перпендикуляре к CD и на расстоянии radius от C
            Vector2f A = C + radius * perpendicular;
            Vector2f B = C - radius * perpendicular;
            return (A, B);
        }
        public override void Update(Vector2f pos, float deltaTime, MapDef map)
        {
            // Найдем точки A и B
            (Vector2f A, Vector2f B) = FindPoints(Position, pos, radius);
            Connections[0] = (A, B);
            Points[0] = A;
            Points[1] = B;
            textureIndex = new();
            CreateIndexTexture(0, 1);
        }
    }
}
