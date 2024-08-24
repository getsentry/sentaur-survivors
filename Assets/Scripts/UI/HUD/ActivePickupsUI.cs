using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ActivePickupsUI : MonoBehaviour
{
    struct ActivePickupState
    {
        public GameObject GameObject;
        public float ExpirationTime;

        public ActivePickupState(GameObject gameObject, float expirationTime)
        {
            this.GameObject = gameObject;
            this.ExpirationTime = expirationTime;
        }

        public void Extend(float expirationTime)
        {
            this.ExpirationTime = expirationTime;
        }
    }

    private Dictionary<int, ActivePickupState> _activePickups =
        new Dictionary<int, ActivePickupState>();

    public void Add(Sprite icon, float duration)
    {
        var iconInstanceId = icon.GetInstanceID();
        if (_activePickups.ContainsKey(iconInstanceId))
        {
            // if the pickup is already active, extend its duration
            _activePickups[iconInstanceId].Extend(Time.time + duration);
            return;
        }

        // get in game height of this object
        float containerHeight = GetComponent<RectTransform>().rect.height;

        GameObject pickupObject = new GameObject("PickupIcon");

        Image imageComponent = pickupObject.AddComponent<Image>();
        imageComponent.sprite = icon;
        imageComponent.rectTransform.sizeDelta = new Vector2(containerHeight, containerHeight);

        pickupObject.transform.SetParent(transform);

        _activePickups.Add(
            icon.GetInstanceID(),
            new ActivePickupState(pickupObject, Time.time + duration)
        );
    }

    private void Update()
    {
        List<int> expiredPickupStateIds = new List<int>();

        foreach (var kvp in _activePickups)
        {
            if (Time.time > kvp.Value.ExpirationTime)
            {
                expiredPickupStateIds.Add(kvp.Key);
            }
        }

        foreach (var expiredPickupId in expiredPickupStateIds)
        {
            Destroy(_activePickups[expiredPickupId].GameObject);
            _activePickups.Remove(expiredPickupId);
        }
    }
}
