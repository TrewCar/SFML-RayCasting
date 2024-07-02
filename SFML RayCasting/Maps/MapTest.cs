using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Enemy;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Maps
{
    internal class MapTest : MapDef
    {
        protected override void InitObjects()
        {
            //"Textures\\breekWall2.jpg"
            VertexObject obj1 = new VertexObject("Name1", new Vector2f(50, 600), new Color(15, 15, 15, 255), 0.5f, true, 1);
            obj1.AddRelativePoint(new Vector2f(0, 0));
            obj1.AddRelativePoint(new Vector2f(400, 0));
            obj1.AddRelativePoint(new Vector2f(400, 50));
            obj1.AddRelativePoint(new Vector2f(0, 50));

            obj1.Points.Reverse();

            obj1.AddConnection(0, 1);
            obj1.AddConnection(1, 2);
            obj1.AddConnection(2, 3);
            obj1.AddConnection(3, 0);
            Objects.Add(obj1);

            //"Textures\\breekWall2.jpg"
            VertexObject obj2 = new VertexObject("Name2", new Vector2f(50, 300), "Textures\\breekWall3.jpg", 1f, false, 1);
			obj2.AddRelativePoint(new Vector2f(0, 0));
			obj2.AddRelativePoint(new Vector2f(400, 0));
			obj2.AddRelativePoint(new Vector2f(400, 50));
			obj2.AddRelativePoint(new Vector2f(0, 50));

			obj2.Points.Reverse();

			obj2.AddConnection(0, 1);
			obj2.AddConnection(1, 2);
			obj2.AddConnection(2, 3);
			obj2.AddConnection(3, 0);
			Objects.Add(obj2);


            Banana obj34 = new Banana("BananaEnemy1", new Vector2f(50, 150), 1);
            Objects.Add(obj34);

            //VertexObject obj2 = new VertexObject("Name1", new Vector2f(50, 600), "Textures\\wolf.jpg", 3, false, 0.5f);
            //obj2.AddRelativePoint(new Vector2f(0, 0));
            //obj2.AddRelativePoint(new Vector2f(400, 0));
            //obj2.AddRelativePoint(new Vector2f(400, 50));
            //obj2.AddRelativePoint(new Vector2f(0, 50));

            //obj2.Points.Reverse();

            //obj2.AddConnection(0, 1);
            //obj2.AddConnection(1, 2);
            //obj2.AddConnection(2, 3);
            //obj2.AddConnection(3, 0);
            //Objects.Add(obj2);


            VertexObject obj3 = new VertexObject("Name3", new Vector2f(50, 50), "Textures\\breekWall2.jpg",1 , false,1);
            obj3.AddRelativePoint(new Vector2f(0, 0));
            obj3.AddRelativePoint(new Vector2f(800, 0));
            obj3.AddRelativePoint(new Vector2f(800, 50));
            obj3.AddRelativePoint(new Vector2f(0, 50));

            obj3.Points.Reverse();

            obj3.AddConnection(0, 1);
            obj3.AddConnection(1, 2);
            obj3.AddConnection(2, 3);
            obj3.AddConnection(3, 0);
            Objects.Add(obj3);

            VertexObject obj4 = new VertexObject("Name4", new Vector2f(50, 800), "Textures\\wolf.jpg",1f , false, 1);
            obj4.AddRelativePoint(new Vector2f(0, 0));
            obj4.AddRelativePoint(new Vector2f(800, 0));
            obj4.AddRelativePoint(new Vector2f(800, 50));
            obj4.AddRelativePoint(new Vector2f(0, 50));

            obj4.Points.Reverse();

            obj4.AddConnection(0, 1);
            obj4.AddConnection(1, 2);
            obj4.AddConnection(2, 3);
            obj4.AddConnection(3, 0);
            Objects.Add(obj4);

            obj4 = new VertexObject("Name5", new Vector2f(50, 800), "Textures\\wolf.jpg", 1f, false, 3);
            obj4.AddRelativePoint(new Vector2f(0, 0));
            obj4.AddRelativePoint(new Vector2f(800, 0));
            obj4.AddRelativePoint(new Vector2f(800, 50));
            obj4.AddRelativePoint(new Vector2f(0, 50));

            obj4.Points.Reverse();

            obj4.AddConnection(0, 1);
            obj4.AddConnection(1, 2);
            obj4.AddConnection(2, 3);
            obj4.AddConnection(3, 0);
            Objects.Add(obj4);

            VertexObject obj5 = new VertexObject("Name6", new Vector2f(800, 50), "Textures\\breekWall2.jpg",1 , false,1);

            obj5.AddRelativePoint(new Vector2f(0, 0));
            obj5.AddRelativePoint(new Vector2f(0, 800));
            obj5.AddRelativePoint(new Vector2f(50, 800));
            obj5.AddRelativePoint(new Vector2f(50, 0));

            obj5.Points.Reverse();

            obj5.AddConnection(0, 1);
            obj5.AddConnection(1, 2);
            obj5.AddConnection(2, 3);
            obj5.AddConnection(3, 0);
            Objects.Add(obj5);

			obj5 = new VertexObject("Name7", new Vector2f(100, 700), "Textures\\breekWall2.jpg", 1, false, 1);

			obj5.AddRelativePoint(new Vector2f(0, 0));
			obj5.AddRelativePoint(new Vector2f(0, 800));
			obj5.AddRelativePoint(new Vector2f(50, 800));
			obj5.AddRelativePoint(new Vector2f(50, 0));

			obj5.Points.Reverse();

			obj5.AddConnection(0, 1);
			obj5.AddConnection(1, 2);
			obj5.AddConnection(2, 3);
			obj5.AddConnection(3, 0);
			Objects.Add(obj5);


			Objects.Add(AbsObject.InstanceCircule("Name8", new Vector2f(400, 400), 20, 50f, "Textures\\wolf.jpg", 1, false));
            //Objects.Add(AbsObject.InstanceCircule("Name6", new Vector2f(400, 200), 4, 50f, "Textures\\breekWall2.jpg", 1, false));
        }
    }
}
