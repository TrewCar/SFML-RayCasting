using SFML.System;
using SFML_RayCasting;
using SFML_RayCasting.Objects;

public class Ray
{
    public Vector2f Direction { get; private set; }
    public List<Collision> Colisions { get; set; } = new List<Collision>();
    public Vector2f StartPos { get; private set; }
    public Vector2f EndPoint { get; private set; }
    public float MaxLen { get; private set; }
    public float DotRay { get; private set; }

    public bool IsColision { get; set; }

    public float Angle { get; private set; }

    public Ray(Vector2f position, Vector2f direction, Vector2f endPoint, float maxLen)
    {
        StartPos = position;
        Direction = direction;
        EndPoint = endPoint;
        MaxLen = maxLen;
        DotRay = MathUtils.Dot(MathUtils.Normalized(direction), MathUtils.Normalized(endPoint));

        Vector2f normalizedDirection = MathUtils.Normalized(direction);
        Angle = MathUtils.RadiansToDegrees((float)Math.Atan2(normalizedDirection.Y, normalizedDirection.X));
    }

    public static Ray CreateRay(Vector2f position, float angle, float length)
    {
        float angleRad = MathUtils.DegreesToRadians(angle);
        Vector2f direction = new Vector2f((float)System.Math.Cos(angleRad), (float)System.Math.Sin(angleRad));
        Vector2f endPoint = position + direction * length;
        return new Ray(position, direction, endPoint, length);
    }
    public static Ray CreateRay(Vector2f position, Vector2f direction, float length)
    {
        Vector2f endPoint = position + MathUtils.Normalized(direction) * length;
        return new Ray(position, direction, endPoint, length);
    }
}