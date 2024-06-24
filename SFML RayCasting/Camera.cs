using SFML.System;
using SFML.Window;
using SFML_RayCasting.Menedgers;

public class Camera
{
    public Vector2f Position { get; set; }
    public float Angle { get; set; }

    protected float previousMouseX;
    protected float previousMouseY;
    protected float velosityMouse = 0.5f;
    protected float velosityMouseY = 0.1f;

    public float zIndex = 0; // позиция по оси прыжка

    private float jumpVelocity = 0; // Скорость прыжка
    private bool isJumping = false; // Находится ли игрок в прыжке
    private const float gravity = -9.81f; // Гравитация (отрицательное значение для падения вниз)
    private float jumpStrength; // Начальная скорость прыжка
    private float groundLevel = 0; // Уровень земли (где игрок стоит)
    private const float desiredJumpHeight = 2.0f; // Желаемая высота прыжка

    public Camera(Vector2f startPosition, float startAngle)
    {
        Position = startPosition;
        Angle = startAngle;
        jumpStrength = (float)Math.Sqrt(2 * Math.Abs(gravity) * desiredJumpHeight);
    }

    public void Move(Vector2f direction)
    {
        Position += direction;
    }

    public void Rotate(float angleOffset)
    {
        Angle += angleOffset;
    }

    public Vector2f GetDirection()
    {
        float angleRad = MathUtils.DegreesToRadians(Angle);
        return new Vector2f((float)System.Math.Cos(angleRad), (float)System.Math.Sin(angleRad));
    }

    public Vector2f GetPerpendicularDirection()
    {
        float angleRad = MathUtils.DegreesToRadians(Angle + 90); // Перпендикулярное направление
        return new Vector2f((float)System.Math.Cos(angleRad), (float)System.Math.Sin(angleRad));
    }
    Vector2f directionMove = new Vector2f();

    public void OnKeyPressed(object sender, KeyEventArgs e)
    {

        if (Keyboard.IsKeyPressed(Keyboard.Key.K))
        {
            if (!isJumping)
                zIndex += 5f;
            //groundLevel += 0.05f;
        }
        if (e.Code == Keyboard.Key.L)
        {
            //if (!isJumping)
            //    zIndex -= 0.05f;
            groundLevel -= 2f;
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
            this.Rotate(mouseDeltaX * rotationSpeed);
        }

        // Обновляем предыдущую позицию мыши
        previousMouseX = e.X;
    }
    public void Tick(float deltaTime, MenedgerRays collision)
    {
        Vector2f directionMove = new Vector2f();
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            directionMove += this.GetDirection() * 500 * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            directionMove += this.GetDirection() * -500 * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            directionMove += this.GetPerpendicularDirection() * -500 * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            directionMove += this.GetPerpendicularDirection() * 500 * deltaTime;

        Vector2f camPos = this.Position;

        if (directionMove != new Vector2f(0, 0))
        {
            Vector2f newPos = camPos + directionMove;
            collision.CalcRay();
            collision.SaveOnlyCollision();
            List<Ray> rays = collision.rays;

            Vector2f adjustedMove = MathUtils.AdjustMovementForCollision(newPos, zIndex, rays, directionMove);

            this.Move(adjustedMove);
        }


        // Проверка нажатия пробела для начала прыжка
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && !isJumping)
        {
            isJumping = true;
            jumpVelocity = jumpStrength;
        }

        if(groundLevel < zIndex && !isJumping)
        {
            isJumping = true;
            jumpVelocity = -1;
        }

        // Обновление позиции и скорости прыжка
        if (isJumping)
        {
            zIndex += jumpVelocity * deltaTime;
            jumpVelocity += gravity * deltaTime;

            // Проверка, если игрок достиг земли
            if (zIndex <= groundLevel)
            {
                zIndex = groundLevel;
                isJumping = false;
                jumpVelocity = 0;

            }
        }
    }
}
