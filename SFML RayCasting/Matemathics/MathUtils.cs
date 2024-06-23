using SFML.System;
using SFML_RayCasting;
using System.Drawing;
using System.Numerics;

public static class MathUtils
{
    public static float RadiansToDegrees(float radians)
    {
        return radians * (180.0f / (float)Math.PI);
    }
    public static float DegreesToRadians(float degrees)
    {
        return degrees * (float)System.Math.PI / 180.0f;
    }

    public static float Dot(Vector2f vec1, Vector2f vec2)
    {
        return vec1.X * vec2.X + vec1.Y * vec2.Y;
    }

    public static Vector2f Normalized(Vector2f vector)
    {
        float length = (float)System.Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        if (length > 0)
        {
            return new Vector2f(vector.X / length, vector.Y / length);
        }
        else
        {
            return new Vector2f(0,0); // Возвращаем вектор с нулевой длиной, если длина исходного вектора была нулевой
        }
    }

    public static Vector2f? RayIntersection(Vector2f rayStart, Vector2f rayDir, Vector2f segStart, Vector2f segEnd)
    {
        // Параметры для уравнения луча: P = rayStart + t * rayDir
        float epsilon = 0.0001f; // Эпсилон для учета погрешности вычислений

        // Вычисляем векторы от начала луча к началу и концу отрезка
        Vector2f segDir = segEnd - segStart;
        Vector2f rayToSegStart = segStart - rayStart;

        // Вычисляем матрицу системы уравнений
        float rayDirCrossSegDir = rayDir.X * segDir.Y - rayDir.Y * segDir.X;

        // Проверяем, параллельны ли луч и отрезок (если кросс-продукт близок к нулю)
        if (System.Math.Abs(rayDirCrossSegDir) < epsilon)
        {
            return null; // Луч и отрезок параллельны или совпадают
        }

        // Находим параметр t для луча, который дает точку пересечения
        float t = ((rayToSegStart.X * segDir.Y - rayToSegStart.Y * segDir.X) / rayDirCrossSegDir);

        // Проверяем, что точка пересечения лежит на луче (t >= 0)
        if (t >= 0)
        {
            // Находим координаты точки пересечения
            Vector2f intersectionPoint = new Vector2f(rayStart.X + t * rayDir.X, rayStart.Y + t * rayDir.Y);
            // Проверяем, лежит ли точка пересечения на отрезке
            float segLength = Distance(segStart, segEnd);
            float distFromSegStart = Distance(segStart, intersectionPoint);
            float distFromSegEnd = Distance(segEnd, intersectionPoint);

            if (distFromSegStart + distFromSegEnd - segLength <= epsilon)
            {
                return intersectionPoint; // Точка пересечения лежит на отрезке
            }
        }

        return null; // Нет пересечения луча с отрезком
    }
    public static float Distance(Vector2f v1, Vector2f v2)
    {
        return (float)Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2));
    }
    public static Vector2f GetReflectedVector(Vector2f direction, Vector2f vert0, Vector2f vert1)
    {
        Vector2f rayDirection = MathUtils.Normalized(direction);

        Vector2f segmentDirection = vert0 - vert1;
        Vector2f segmentNormal = new Vector2f(-segmentDirection.Y, segmentDirection.X);
        segmentNormal = MathUtils.Normalized(segmentNormal);

        if (MathUtils.Dot(rayDirection, segmentNormal) > 0)
        {
            segmentNormal = -segmentNormal;
        }

        return MathUtils.Reflect(rayDirection, segmentNormal);
    }

    public static Vector2f Cross(Vector2f v1, Vector2f v2)
    {
        return new Vector2f(v1.X * v2.Y - v1.Y * v2.X, v1.Y * v2.X - v1.X * v2.Y);
    }
    public static Vector2f Reflect(Vector2f direction, Vector2f normal)
    {
        return direction - 2 * Dot(direction, normal) * normal;
    }
    public static Vector2f StepToPoint(Vector2f v1, Vector2f v2, float step)
    {

        // Находим вектор от a к b
        float dx = v2.X - v1.X;
        float dy = v2.Y - v1.Y;

        // Находим длину вектора ab
        float length = MathF.Sqrt(dx * dx + dy * dy);

        // Нормализуем вектор ab (единичный вектор в направлении от a к b)
        float ux = dx / length;
        float uy = dy / length;

        // Находим новую точку c, сдвинувшись на distance вдоль вектора ab
        float newX = v1.X + step * ux;
        float newY = v1.Y + step * uy;

        return new Vector2f(newX, newY);
    }
    public static Vector2f AdjustMovementForCollision(Vector2f newPos, List<Ray> rays, Vector2f originalMove)
    {
        Vector2f adjustedMove = originalMove;

        foreach (Ray ray in rays)
        {
            foreach (Collision col in ray.Colisions)
            {
                if (MathUtils.Distance(newPos, col.Pos) < 10) // Определить столкновение
                {
                    Vector2f normal = col.NornalCollison.Item1 - col.NornalCollison.Item2;
                    normal = MathUtils.Normalized(new Vector2f(-normal.Y, normal.X));

                    // Проецируем направление движения на нормаль поверхности, чтобы получить направление скольжения
                    float dotProduct = MathUtils.Dot(adjustedMove, normal);
                    Vector2f slideDirection = adjustedMove - dotProduct * normal;

                    // Корректируем направление движения, чтобы включить скольжение
                    adjustedMove = slideDirection;

                    break;
                }
            }
        }

        return adjustedMove;
    }
}
