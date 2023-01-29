using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoExplosion : MonoBehaviour
{
    public GameObject ex1, ex2, ex3;
    float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 30f / 136f)
        {
            ex1.SetActive(false);
            ex2.SetActive(true);
        }
        if (time >= 60f / 136f)
        {
            if (ex3)
            {
                ex2.SetActive(false);
                ex3.SetActive(true);
            }
            else Destroy(gameObject);
        }
        if (time >= 90f / 136f) Destroy(gameObject);
    }
}
