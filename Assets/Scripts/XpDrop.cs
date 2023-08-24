using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class XpDrop : MonoBehaviour
{
    private int _xp = 10;
    private bool _moving = false;

    [SerializeField]
    private float _detectPlayerDistance = 3f;

    [SerializeField]
    private float _moveSpeed = 5f;

    public void SetXp(int xp)
    {
        _xp = xp;
    }

    void Update() {
        var player = Player.Instance;
        if (player == null) {
            return;
        }
        // detect if player is with 1 unit of the pickup
        if (!_moving && Vector2.Distance(player.transform.position, transform.position) < _detectPlayerDistance)
        {
            _moving = true;
        }

        if (_moving) {
            // move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, _moveSpeed * Time.deltaTime);
        }

    }
    
    // on trigger handler
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerHitbox")
        {
            // var player = other.transform.parent.gameObject.GetComponent<Player>();
            
            EventManager.TriggerEvent("XpEarned", new EventData(_xp));

            Destroy(gameObject);
        }
    }
}
