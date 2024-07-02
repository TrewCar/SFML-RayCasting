using SFML.System;
using SFML.Window;
using SFML_RayCasting.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Player : Camera
{
    public Player(Vector2f startPosition, float startAngle) : base(startPosition, startAngle)
    {
    }
    public override void Tick(float deltaTime, MapDef map)
    {
        Vector2f directionMove = new Vector2f();
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            directionMove += this.GetDirection() * speedVelosity * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            directionMove += this.GetDirection() * -speedVelosity * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            directionMove += this.GetPerpendicularDirection() * -speedVelosity * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            directionMove += this.GetPerpendicularDirection() * speedVelosity * deltaTime;

        AdjastiveMove(deltaTime, map, directionMove);
    }
    public override void OnKeyPressed(object sender, KeyEventArgs e)
    {

        if (Keyboard.IsKeyPressed(Keyboard.Key.K))
        {
            //if (!isJumping)
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
    public override void OnMouseMoved(object sender, MouseMoveEventArgs e)
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
}

