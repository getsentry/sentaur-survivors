using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much this heals the player for, if applicable")]
    private int _healAmount = 30;

    [SerializeField]
    [Tooltip("How much score this pickup is worth")]
    private int _scoreValue = 50;

    [SerializeField]
    [Tooltip("How long the effect lasts (instants have 0 duration)")]
    private float _effectDuration = 0;

    [SerializeField]
    private PickupType _pickupType;

    public enum PickupType
    {
        Hotdog,
        Skateboard,
        Umbrella,
        Money,
        Mozart
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
                    player.SpeedUp(5, _effectDuration);
                    break;
                case PickupType.Umbrella:
                    player.ReduceDamage(0.5f, _effectDuration);
                    break;
                case PickupType.Money: // only increments score, nothing else
                default:
                    break;
            }

            // Destroy the pickup
            Destroy(this.gameObject);

            // trigger an event that lets people know this was picked up
            List<object> eventData = new List<object>
            {
                _scoreValue,
                _pickupType.ToString(),
                _effectDuration
            };
            EventManager.TriggerEvent("PickupGrabbed", new EventData(eventData));
        }
    }
}
