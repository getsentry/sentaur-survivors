using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raven : MonoBehaviour
{
    [Tooltip("How fast the raven moves")]
    [SerializeField]
    private float _speed = 20.0f;

    [SerializeField]
    [Tooltip("How much damage this raven does to an enemy")]
    public static int Damage = 10;

    private Vector3 _direction;
    private GameObject _player;
    private float _distanceOutsidePlayer = 2.0f;

    void Awake()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void TargetClosestEnemy() 
    {
        GameObject target = FindClosestEnemy();
        Vector3 direction = target.transform.position - _player.transform.position;
        SetDirection(direction);
        transform.position = _player.transform.position + direction.normalized * _distanceOutsidePlayer;
    }

    private GameObject FindClosestEnemy() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float distance = Mathf.Infinity;
        Vector3 position = _player.transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestEnemy = enemy;
                distance = curDistance;
            }
        }
        return closestEnemy;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        // rotate the raven to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if the raven collides with an enemy, destroy the enemy
        if (other.gameObject.tag == "Enemy")
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            DamageEnemy(enemy);

            // Destroy the raven
            Destroy(this.gameObject);
        } else if (other.gameObject.tag == "Barrier") {
            // Destroy the raven
            Destroy(this.gameObject);
        }
    }

    // Deal damage to the enemy because they were hit by a dart
    private void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(Damage);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    } 
}
