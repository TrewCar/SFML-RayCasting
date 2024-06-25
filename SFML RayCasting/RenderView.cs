using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML_RayCasting;
using SFML_RayCasting.Maps;
using SFML_RayCasting.Menedgers;
using SFML_RayCasting.Utils;
using System.Numerics;

public class RenderView
{
    private MapDef map = new MapTest();

    private int maxRecurs = 15;

    private PrimitiveType primitiveMap = PrimitiveType.LineStrip;

    private MenedgerRays raysView;

    private delegate void PreviewCalcRay();
    private PreviewCalcRay OnPreviewCalcRay;

    private delegate void TickFrame(List<Ray> view, float deltaTime);
    private TickFrame Tick;

    public RenderView(MapDef map)
    {
        this.map = map;

        OnPreviewCalcRay += map.OnPreviewCalcRay;
        Tick += map.Tick;

        WindowMenedger.InstanceWindow(map.width, map.height, "2D Ray Rendering");
        WindowMenedger.window.SetVerticalSyncEnabled(true);
        WindowMenedger.window.Closed += (sender, e) => WindowMenedger.window.Close();
        WindowMenedger.window.KeyPressed += map.OnKeyPressed;
        WindowMenedger.window.MouseMoved += map.OnMouseMoved;

    }

    public void Run()
    {
        FPSCounter fpsCounter = new FPSCounter();
        raysView = new MenedgerRays(map, maxRecurs, true);
        while (WindowMenedger.IsOpen)
        {
            WindowMenedger.window.DispatchEvents();
            WindowMenedger.Clear(Color.Black);

            OnPreviewCalcRay.Invoke();

            this.raysView.CalcRay();


            Tick.Invoke(raysView.rays, fpsCounter.deltaTime);

            DrawMap();


            WindowMenedger.window.Display();

            fpsCounter.Update();
        }
    }

    private void DrawMap()
    {
        RenderWindowView.Render(map, raysView.rays);
        //map.Draw();
        //raysView.DrawRays(primitiveMap);
    }
}
