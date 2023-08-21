using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // move towards the player character
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        Debug.Log("OnTriggerenter");
    }

    
    // a collision handler that is called when the enemy collides with another object
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        Debug.Log("OnCollisionEnter");

     
    }
    
}
