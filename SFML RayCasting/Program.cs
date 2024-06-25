using SFML.System;
using SFML_RayCasting.Maps;

class Program
{
    static void Main()
    {
        int width = 1620;
        int height = 1024;
        float step = 20;
        float rayLength = 2000;

        Camera camera = new Camera(new Vector2f(300, 300), 0.0f);
        MapDef map = new MapTest();

        map.width = width;
        map.height = height;
        map.camera = camera;
        map.step = step;
        map.rayLength = rayLength;


        RenderView renderView = new RenderView(map);

        renderView.Run();
    }
}
