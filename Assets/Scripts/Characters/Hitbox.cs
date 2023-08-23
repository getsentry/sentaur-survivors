using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Enemy _parentEnemy;

    public Enemy Enemy => _parentEnemy;

    void Awake() {
        _parentEnemy = transform.parent.GetComponent<Enemy>();
    }

}
