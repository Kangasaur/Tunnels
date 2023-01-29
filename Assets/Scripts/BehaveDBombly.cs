using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaveDBombly : MonoBehaviour
{
    public GameObject explosion, exRed;
    AudioSource explosionSound;

    private void Start()
    {
        explosionSound = GameObject.Find("Canvas/AudioSystem/ExplsnDBomb").GetComponent<AudioSource>();
    }

    void Detonate()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, 2f))
        {
            if (collider.gameObject.tag == "world") GameObject.Find("Cube maker").SendMessage("DeleteVoxel", collider.gameObject);
            else if (collider.gameObject.tag == "ship")
            {
                if (collider.gameObject.name == "ship2(Clone)")
                {
                    GameObject ex = Instantiate(exRed, collider.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    ex.transform.localScale = Vector3.one * 1.5f;
                    GameObject.Find("Canvas").SendMessage("IncrementKillcount");
                }
                else
                {
                    GameObject ex = Instantiate(explosion, collider.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    ex.transform.localScale = Vector3.one * 1.5f;
                }
                Destroy(collider.gameObject);
            }
        }
        GameObject a = Instantiate(explosion);
        a.transform.localScale = Vector3.one * 4f;
        explosionSound.Play();
        Destroy(gameObject);
    }
}
