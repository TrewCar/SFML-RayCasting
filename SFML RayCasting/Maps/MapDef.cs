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

        public virtual Vector2f InputKey(object sender, KeyEventArgs e)
        {
            Vector2f directionMove = new Vector2f(0, 0);

            if (e.Code == Keyboard.Key.W)
                directionMove += camera.GetDirection() * 10;
            if (e.Code == Keyboard.Key.S)
                directionMove += camera.GetDirection() * -10;
            if (e.Code == Keyboard.Key.A)
                directionMove += camera.GetPerpendicularDirection() * -10;
            if (e.Code == Keyboard.Key.D)
                directionMove += camera.GetPerpendicularDirection() * 10;



            if (e.Code == Keyboard.Key.K)
                camera.Rotate(-5);
            if (e.Code == Keyboard.Key.L)
                camera.Rotate(5);

            return directionMove;
        }

        public void OnKeyPressed(object sender, KeyEventArgs e)
        {

            Vector2f camPos = camera.Position;

            Vector2f directionMove = InputKey(sender, e);

            if (directionMove != new Vector2f(0, 0))
            {
                Vector2f newPos = camPos + directionMove;
                var collision = new MenedgerRays(this);
                collision.CalcRay();
                collision.SaveOnlyCollision();
                List<Ray> rays = collision.rays;

                Vector2f adjustedMove = MathUtils.AdjustMovementForCollision(newPos, rays, directionMove);

                camera.Move(adjustedMove);
            }
        }
        public virtual void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            // Определяем разницу в позиции мыши по горизонтали
            float mouseDeltaX = e.X - previousMouseX;

            // Изменяем угол поворота камеры в зависимости от движения мыши
            if (mouseDeltaX != 0)
            {
                float rotationSpeed = velosityMouse; // Скорость вращения камеры с помощью мыши
                camera.Rotate(mouseDeltaX * rotationSpeed);
            }

            // Обновляем предыдущую позицию мыши
            previousMouseX = e.X;
        }

        public virtual void OnPreviewCalcRay()
        {
            return;
        }
        public virtual void Tick(List<Ray> view)
        {
            return;
        }
    }
}
