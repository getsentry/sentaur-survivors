using UnityEngine;
using UnityEngine.UI;

class ActivePickupsUI : MonoBehaviour
{
    public void Add(Sprite icon, float duration)
    {
        // get in game height of this object
        float containerHeight = GetComponent<RectTransform>().rect.height;

        GameObject pickupObject = new GameObject("Pickup");

        Image imageComponent = pickupObject.AddComponent<Image>();
        imageComponent.sprite = icon;
        imageComponent.rectTransform.sizeDelta = new Vector2(containerHeight, containerHeight);

        pickupObject.transform.SetParent(transform);

        // disable this object after duration
        Destroy(pickupObject, duration);
    }
}
