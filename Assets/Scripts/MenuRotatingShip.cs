using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotatingShip : MonoBehaviour
{
    float rSpeed = 5f;
    
    void Update()
    {
        transform.Rotate(Vector3.forward * rSpeed * Time.deltaTime);
    }
}
