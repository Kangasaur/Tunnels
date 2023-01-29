using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMenuEffect : MonoBehaviour
{
    float hspeed = -0.2f;
    float vspeed = -0.075f;
    
    void Update()
    {
        transform.Translate(hspeed * Time.deltaTime, vspeed * Time.deltaTime, 0.0f);
    }
}
