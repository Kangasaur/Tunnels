using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    public GameObject center;
    public float multiplier;
    private float Zoom;
    private float Distance;
    private float zoomspeed;
    void Update()
    {
        if (center)
        {
            Zoom = Input.GetAxis("Mouse ScrollWheel");
            Distance = Vector3.Distance(transform.position, center.transform.position);
            zoomspeed = Zoom * multiplier * Distance;
            if (Distance > 5 && Distance < 100)
            {
                transform.Translate(Vector3.forward * zoomspeed);
            }
            if (Distance < 5 && zoomspeed < 0)
            {
                transform.Translate(Vector3.forward * zoomspeed);
            }
            if (Distance > 100 && zoomspeed > 0)
            {
                transform.Translate(Vector3.forward * zoomspeed);
            }
        }
    }
}
