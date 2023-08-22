using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private int _healAmount = 30;

    [SerializeField]
    private int _scoreValue = 50;

    private enum PickupType 
    {
        Hotdog,
        Skateboard,
        Umbrella
    }

    private PickupType[] pickupTypesArray = Enum.GetValues(typeof(PickupType)) as PickupType[];
    private PickupType pickupType;
    private static System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        // randomize pickup type
        pickupType = (PickupType) pickupTypesArray.GetValue(random.Next(pickupTypesArray.Length));

        switch (pickupType)
        {
            case PickupType.Hotdog:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case PickupType.Skateboard:
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case PickupType.Umbrella:
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the player touches the pickup, destroy the pickup
        if (other.gameObject.name == "Player")
        {
            var player = other.gameObject.GetComponent<Player>();

            switch (pickupType)
            {
                case PickupType.Hotdog: 
                    player.HealDamage(_healAmount);
                    break;
                case PickupType.Skateboard:
                    player.SpeedUp(10, true);
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
            EventManager.TriggerEvent("PickupGrabbed", new EventData(_scoreValue));
        }
    }
}
