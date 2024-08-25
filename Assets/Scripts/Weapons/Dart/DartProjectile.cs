using UnityEngine;

public class DartProjectile : ProjectileBase
{
    [SerializeField]
    private float _areaOfEffectRange = 0.25f;

    private Vector3 _direction;
    private int _damage;
    private float _speed;
    private float _areaOfEffectModifier;

    public void Initialize(int damage, float speed, float areaOfEffectModifier = 1.0f)
    {
        _damage = damage;
        _speed = speed;
        _areaOfEffectModifier = areaOfEffectModifier;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
        _direction.z = 0; // don't move in the z direction

        // rotate the dart to face the direction it's moving
        var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _rigidbody2D.velocity = Vector3.Normalize(_direction) * _speed;
    }

    // Deal damage to the enemy because they were hit by a dart
    override protected void DamageEnemy(Enemy initialEnemy)
    {
        initialEnemy.TakeDamage(_damage);
        // why 5000? -- the result of experimenting with different values (!)
        initialEnemy.Knockback(_direction, 5000);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            initialEnemy.transform.position,
            _areaOfEffectRange,
            Vector2.zero
        );
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyHit = hit.collider.gameObject.GetComponent<Enemy>();
                // dont hit initial enemy twice
                if (enemyHit != initialEnemy)
                {
                    enemyHit.TakeDamage(_damage);
                }
            }
        }
    }
}
