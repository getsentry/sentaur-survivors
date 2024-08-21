using System;
using UnityEngine;

public class LinearEnemy : Enemy
{
    Vector2 _direction;

    float _timeAtSpawn;

    Vector3 _targetScale;

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
        _targetScale = transform.localScale;

        _timeAtSpawn = Time.time;
        SetDirection(Direction.Up);
        transform.localScale = new Vector3(0, 0);
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

    protected override void Update()
    {
        base.Update();
        var timeElapsed = Time.time - _timeAtSpawn;
        transform.localScale = _targetScale * Math.Min(1, timeElapsed);
    }

    protected override Vector2 DetermineDirection(GameObject player)
    {
        // "ease out" this enemy's movement so they start slow at first, then
        // speed up to max speed
        var timeElapsed = Time.time - _timeAtSpawn;
        timeElapsed = Math.Min(1, timeElapsed);

        // ignores player and moves in a straight line
        return Vector3.Normalize(_direction) * timeElapsed;
    }
}
