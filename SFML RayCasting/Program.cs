using SFML.System;
using SFML_RayCasting.Maps;

class Program
{
    public static float maxDistRay = 900;
    static void Main()
    {
        int width = 1620;
        int height = 1024;
        float step = 3;
        float rayLength = maxDistRay;

        Camera camera = new Player(new Vector2f(130, 130), 0.0f);
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
