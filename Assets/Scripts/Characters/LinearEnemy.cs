using System;
using UnityEngine;

public class LinearEnemy : Enemy
{
    Vector2 _direction;

    protected override void Awake()
    {
        base.Awake();

        // initialize a random direction
        System.Random rnd = new System.Random();
        switch (rnd.Next(0, 4))
        {
            case 0: // up
                _direction = new Vector3(0, 1);
                break;
            case 1: // down
                _direction = new Vector3(0, -1);
                break;
            case 2: // left
                _direction = new Vector3(-1, 0);
                break;
            case 3: // right
                _direction = new Vector3(1, 0);
                break;
        }
    }

    protected override Vector2 DetermineDirection(GameObject player)
    {
        return Vector3.Normalize(_direction);
    }
}
