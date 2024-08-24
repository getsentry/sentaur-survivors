using System.Collections;
using UnityEngine;

public class MozartPickup : PickupBase
{
    protected override void OnCollect(Player player)
    {
        StartCoroutine(SetAllXpDropsToMoving());
    }

    // empty co-routine
    private IEnumerator SetAllXpDropsToMoving()
    {
        // get all xp drops and set moving = true
        var xpDrops = GameObject.FindGameObjectsWithTag("XpDrop");
        foreach (var xpDrop in xpDrops)
        {
            xpDrop.GetComponent<XpDrop>().SetMoving();
        }
        yield return null;
    }
}
