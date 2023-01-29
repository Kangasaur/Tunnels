using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    float rot = 0f;
    float rotspeed = 1.5f;
    bool initialized = false;
    public GameObject ship;
    
    void Update()
    {
        if (Input.GetKey("q") && Input.GetKey("e")) rot = 0f;
        else if (Input.GetKey("q")) rot = 1f;
        else if (Input.GetKey("e")) rot = -1f;
        else rot = 0f;

        transform.Rotate(0f, rot * rotspeed, 0f);
        if (ship)
        {
            transform.position = ship.transform.position;
            if (ship.transform.rotation.eulerAngles.y != 0f && !initialized)
            {
                transform.rotation = Quaternion.Euler(0f, ship.transform.rotation.eulerAngles.y-90, 0f);
                initialized = true;
            }
        }
    }
}
