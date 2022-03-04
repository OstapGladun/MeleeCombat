using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    
    void Update()
    {
        //fix this, it only works in 16:9 resolution
        transform.position = Player.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        if (transform.position.x > 47)
        {
            transform.position = new Vector3(47f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -2)
        {
            transform.position = new Vector3(-2, transform.position.y, transform.position.z);
        }
        if (transform.position.y > 34.75)
        {
            transform.position = new Vector3(transform.position.x, 34.75f , transform.position.z);
        }
        if (transform.position.y < -4.75)
        {
            transform.position = new Vector3(transform.position.x, -4.75f, transform.position.z);
        }   
    }
}