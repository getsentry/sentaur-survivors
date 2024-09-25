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
    [Tooltip("The text that will be displayed in the UI when this pickup is collected")]
    protected string _effectText;

    public Sprite Icon => GetComponent<SpriteRenderer>().sprite;

    protected virtual void OnCollect(Player player) { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the player touches the pickup, destroy the pickup
        if (other.gameObject.name == "Player")
        {
            var player = other.gameObject.GetComponent<Player>();

            OnCollect(player);

            SoundManager.Instance.PlayPickupSound();

            Player.Instance.SpawnPlayerText(GetEffectText());

            // Destroy the pickup
            Destroy(this.gameObject);

            // trigger an event that lets people know this was picked up
            List<object> eventData = new List<object> { _scoreValue, gameObject, _effectDuration };
            EventManager.TriggerEvent("PickupGrabbed", new EventData(eventData));
        }
    }

    protected virtual string GetEffectText()
    {
        return _effectText;
    }
}
