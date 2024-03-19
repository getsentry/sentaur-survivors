using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private int _healAmount = 30;

    [SerializeField]
    private int _scoreValue = 50;

    [SerializeField]
    private PickupType _pickupType;

    private enum PickupType
    {
        Hotdog,
        Skateboard,
        Umbrella
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the player touches the pickup, destroy the pickup
        if (other.gameObject.name == "Player")
        {
            var player = other.gameObject.GetComponent<Player>();

            switch (_pickupType)
            {
                case PickupType.Hotdog:
                    player.HealDamage(_healAmount);
                    break;
                case PickupType.Skateboard:
                    player.SpeedUp(5, true);
                    break;
                case PickupType.Umbrella:
                    player.ReduceDamage(0.5f, true);
                    break;
                default:
                    break;
            }

            // Destroy the pickup
            Destroy(this.gameObject);

            // trigger an event that lets people know this was picked up
            List<object> eventData = new List<object> { _scoreValue, _pickupType.ToString() };
            EventManager.TriggerEvent("PickupGrabbed", new EventData(eventData));
        }
    }
}
