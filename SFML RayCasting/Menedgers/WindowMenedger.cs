using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Menedgers
{
    public static class WindowMenedger
    {
        public static RenderWindow window { get; private set; } = null;

        public static bool Open = true;

        public static bool IsOpen => window.IsOpen && Open;
        public static float Widht;
        public static float Height;

        public static void InstanceWindow(float width, float height, string title)
        {
            window = new RenderWindow(new VideoMode((uint)width, (uint)height), title);
            Widht = width;
            Height = height;
        }
        public static void DrawVertex(Vertex[] points, PrimitiveType type)
        {
            IfNull();
            window.Draw(points, type);
        }
        public static void Clear(Color color)
        {
            IfNull();
            window.Clear(color);
        }

        private static void IfNull()
        {
            if (window == null)
                throw new NullReferenceException("Window is not instand");
        }
    }
}
