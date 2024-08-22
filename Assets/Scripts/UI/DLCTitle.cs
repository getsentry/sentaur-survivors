using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLCTitle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The strength of the floating effect")]
    private float floatStrength = 20f; // The strength of the floating effect

    [SerializeField]
    [Tooltip("Float effect speed")]
    private float floatSpeed = 2f; // Float effect speed

    private float originalY; // Store the original Y position of the GameObject

    void Start()
    {
        originalY = transform.position.y; // Store the original Y position
    }

    void Update()
    {
        // Calculate the new Y position based on the original position and a sine wave
        float newY = originalY + (Mathf.Sin(Time.time * floatSpeed) * floatStrength);

        // Update the GameObject's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
