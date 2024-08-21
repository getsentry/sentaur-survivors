using System;
using UnityEngine;

public class LinearEnemy : Enemy
{
    Vector2 _direction;

    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    protected override void Awake()
    {
        base.Awake();
        SetDirection(Direction.Up);
    }

    public void SetDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                _direction = new Vector3(0, 1);
                break;
            case Direction.Down:
                _direction = new Vector3(0, -1);
                break;
            case Direction.Left:
                _direction = new Vector3(-1, 0);
                break;
            case Direction.Right:
                _direction = new Vector3(1, 0);
                break;
        }
    }

    protected override Vector2 DetermineDirection(GameObject player)
    {
        return Vector3.Normalize(_direction);
    }
}
