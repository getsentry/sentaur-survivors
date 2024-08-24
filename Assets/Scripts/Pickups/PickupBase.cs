using System.Collections.Generic;
using UnityEngine;

public class PickupBase : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much score this pickup is worth")]
    protected int _scoreValue = 50;

    [SerializeField]
    [Tooltip("How long the effect lasts (instants have 0 duration)")]
    protected float _effectDuration = 0;

    [SerializeField]
    protected PickupType _pickupType;

    public enum PickupType
    {
        Hotdog,
        Skateboard,
        Umbrella,
        Money,
        Mozart
    }

    protected virtual void OnCollect(Player player) { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the player touches the pickup, destroy the pickup
        if (other.gameObject.name == "Player")
        {
            var player = other.gameObject.GetComponent<Player>();

            OnCollect(player);

            // Destroy the pickup
            Destroy(this.gameObject);

            // trigger an event that lets people know this was picked up
            List<object> eventData = new List<object>
            {
                _scoreValue,
                // classname to string
                _pickupType.ToString(),
                _effectDuration
            };
            EventManager.TriggerEvent("PickupGrabbed", new EventData(eventData));
        }
    }
}
