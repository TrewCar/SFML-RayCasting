using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML_RayCasting.Menedgers;
using SFML_RayCasting.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML_RayCasting.Maps
{
    public abstract class MapDef
    {

        public int width;
        public int height;
        public float step;
        public float rayLength;
        public Camera camera;
        public float Pov = 90.0f;
        public bool OnCollison = true;

        protected float previousMouseX;
        protected float velosityMouse = 0.5f;

        public MapDef()
        {
            InitObjects();
        }

        protected abstract void InitObjects();
        public List<AbsObject> Objects = new List<AbsObject>();
        public void Draw()
        {
            foreach (var obj in Objects)
            {
                obj.Draw();
            }
        }

        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            camera.OnKeyPressed(sender, e);
        }
        public virtual void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            camera.OnMouseMoved(sender, e);
        }

        public virtual void OnPreviewCalcRay()
        {
            return;
        }
        public virtual void Tick(List<Ray> view, float deltaTime)
        {
            camera.Tick(deltaTime, new MenedgerRays(this));
            return;
        }
    }
}
