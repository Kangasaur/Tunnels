using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaveMinelike : MonoBehaviour
{
    public GameObject explosion, exRed, exBlue;
    Collider[] collidingObjects;
    AudioSource explosionSound;

    void Start()
    {
        collidingObjects = Physics.OverlapSphere(transform.position, 0.375f);
        explosionSound = GameObject.Find("Canvas/AudioSystem/ExplsnTurret").GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (Physics.OverlapSphere(transform.position, 0.375f).Length != collidingObjects.Length)
        {
            foreach (Collider collider in Physics.OverlapSphere(transform.position, 1.5f))
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
                        GameObject ex = Instantiate(exBlue, collider.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                        ex.transform.localScale = Vector3.one * 1.5f;
                    }
                    Destroy(collider.gameObject);
                }
            }
            GameObject a = Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
            a.transform.localScale = Vector3.one * 3f;
            explosionSound.Play();
            Destroy(gameObject);
        }
    }
}
