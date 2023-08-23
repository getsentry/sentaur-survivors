using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpDrop : MonoBehaviour
{
    private int _xp = 10;

    public void SetXp(int xp)
    {
        _xp = xp;
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
