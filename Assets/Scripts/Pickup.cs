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

    [SerializeField]
    [Tooltip("Sprite for the hotdog pickup")]
    private Sprite _hotdogSprite;

    [SerializeField]
    [Tooltip("Sprite for the skateboard pickup")]
    private Sprite _skateboardSprite;

    [SerializeField]
    [Tooltip("Sprite for the umbrella pickup")]
    private Sprite _umbrellaSprite;

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
                gameObject.GetComponent<SpriteRenderer>().sprite = _hotdogSprite;
                break;
            case PickupType.Skateboard:
                gameObject.GetComponent<SpriteRenderer>().sprite = _skateboardSprite;
                break;
            case PickupType.Umbrella:
                gameObject.GetComponent<SpriteRenderer>().sprite = _umbrellaSprite;
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
            List<object> eventData = new List<object>{_scoreValue, pickupType.ToString()};
            EventManager.TriggerEvent("PickupGrabbed", new EventData(eventData));
        }
    }
}
