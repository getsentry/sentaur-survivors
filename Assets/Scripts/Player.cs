using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * 5,
            transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * 5,
            transform.position.z
        );
    }
}
