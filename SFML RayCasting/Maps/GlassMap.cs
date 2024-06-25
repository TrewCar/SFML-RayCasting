using SFML.Graphics;
using SFML.System;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Maps
{
	internal class GlassMap : MapDef
	{
		protected override void InitObjects()
		{
			//"Textures\\breekWall2.jpg"
			VertexObject obj1 = new VertexObject("Name1", new Vector2f(50, 600), new Color(15, 15, 15, 255), 1f, true, 1);
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

			VertexObject obj2 = new VertexObject("Name1", new Vector2f(50, 250), new Color(15, 15, 15, 255), 1f, true, 1);
			obj2.AddRelativePoint(new Vector2f(0, 0));
			obj2.AddRelativePoint(new Vector2f(0, 400));
			obj2.AddRelativePoint(new Vector2f(50, 400));
			obj2.AddRelativePoint(new Vector2f(50, 0));

			obj2.Points.Reverse();

			obj2.AddConnection(0, 1);
			obj2.AddConnection(1, 2);
			obj2.AddConnection(2, 3);
			obj2.AddConnection(3, 0);
			Objects.Add(obj2);

			VertexObject obj3 = new VertexObject("Name1", new Vector2f(400, 250), new Color(15, 15, 15, 255), 1f, true, 1);
			obj3.AddRelativePoint(new Vector2f(0, 0));
			obj3.AddRelativePoint(new Vector2f(0, 400));
			obj3.AddRelativePoint(new Vector2f(50, 400));
			obj3.AddRelativePoint(new Vector2f(50, 0));

			obj3.Points.Reverse();

			obj3.AddConnection(0, 1);
			obj3.AddConnection(1, 2);
			obj3.AddConnection(2, 3);
			obj3.AddConnection(3, 0);
			Objects.Add(obj3);

			VertexObject obj4 = new VertexObject("Name1", new Vector2f(50, 250), new Color(15, 15, 15, 255), 1f, true, 1);
			obj4.AddRelativePoint(new Vector2f(0, 0));
			obj4.AddRelativePoint(new Vector2f(400, 0));
			obj4.AddRelativePoint(new Vector2f(400, 50));
			obj4.AddRelativePoint(new Vector2f(0, 50));

			obj4.Points.Reverse();

			obj4.AddConnection(0, 1);
			obj4.AddConnection(1, 2);
			obj4.AddConnection(2, 3);
			obj4.AddConnection(3, 0);
			Objects.Add(obj4);

			Objects.Add(AbsObject.InstanceCircule("Name5", new Vector2f(400, 300), 20, 50f, "Textures\\wolf.jpg", 1, false));
			Objects.Add(AbsObject.InstanceCircule("Name5", new Vector2f(100, 300), 20, 50f, "Textures\\wolf.jpg", 1, false));

			Objects.Add(AbsObject.InstanceCircule("Name5", new Vector2f(400, 600), 20, 50f, "Textures\\wolf.jpg", 1, false));
			Objects.Add(AbsObject.InstanceCircule("Name5", new Vector2f(100, 600), 20, 50f, "Textures\\wolf.jpg", 1, false));

		}
	}
}
