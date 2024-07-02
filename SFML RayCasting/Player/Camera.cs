using SFML.System;
using SFML.Window;
using SFML_RayCasting.Maps;
using SFML_RayCasting.Menedgers;

public class Camera
{
    public Vector2f Position { get; set; }
    public float Angle { get; set; }

    protected float previousMouseX;
    protected float previousMouseY;
    protected float velosityMouse = 0.5f;
    protected float velosityMouseY = 0.1f;

    protected float speedVelosity = 300;

    public float zIndex = 0; // позиция по оси прыжка

    protected float jumpVelocity = 0; // Скорость прыжка
    protected bool isJumping = false; // Находится ли игрок в прыжке
    protected const float gravity = -9.81f; // Гравитация (отрицательное значение для падения вниз)
    protected float jumpStrength; // Начальная скорость прыжка
    protected float groundLevel = 0; // Уровень земли (где игрок стоит)
    protected const float desiredJumpHeight = 2.0f; // Желаемая высота прыжка

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
	private float upLevel;

	public virtual void OnKeyPressed(object sender, KeyEventArgs e)
    {
    }
    public virtual void OnMouseMoved(object sender, MouseMoveEventArgs e)
    {
    }
    public virtual void Tick(float deltaTime, MapDef map)
    {

    }
    protected void AdjastiveMove(float deltaTime, MapDef map, Vector2f directionMove)
    {
        Vector2f camPos = this.Position;

        if (directionMove != new Vector2f(0, 0))
        {
            Vector2f newPos = camPos + directionMove;
            MenedgerRays collision = new MenedgerRays(map);

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

        if (groundLevel < zIndex && !isJumping || zIndex > upLevel)
        {
            isJumping = true;
            jumpVelocity = -1;
        }
        var res = CollisionUpDownMen.CalczIndex(camPos, zIndex, map);
        if (res.down > 0)
            groundLevel = res.down + 1;
        else
            groundLevel = 0;

        if (res.up > 0)
            upLevel = res.up - 2;
        else
            upLevel = float.MaxValue;
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
