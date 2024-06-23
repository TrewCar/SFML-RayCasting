using SFML.System;

public class Camera
{
    public Vector2f Position { get; set; }
    public float Angle { get; set; }

    public Camera(Vector2f startPosition, float startAngle)
    {
        Position = startPosition;
        Angle = startAngle;
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
}
