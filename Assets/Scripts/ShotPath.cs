using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPath : MonoBehaviour
{
    float speed = 20f;
    float maxRange = 50f;
    public GameObject explosion, exRed, exBlue;
    float distTravelled = 0;
    RaycastHit hit;
    AudioSource turSound, enemSound, plrSound;

    void Start()
    {
        turSound = GameObject.Find("Canvas/AudioSystem/ExplsnTurret").GetComponent<AudioSource>();
        enemSound = GameObject.Find("Canvas/AudioSystem/ExplsnEnemy").GetComponent<AudioSource>();
        plrSound = GameObject.Find("Canvas/AudioSystem/ExplsnPlayer").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, speed * Time.deltaTime))
        {
            if (hit.collider.gameObject.tag == "ship")
            {
                if (hit.collider.gameObject.name == "ship2(Clone)")
                {
                    enemSound.Play();
                    GameObject ex = Instantiate(exRed, hit.collider.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    ex.transform.localScale = Vector3.one * 1.5f;
                    GameObject.Find("Canvas").SendMessage("IncrementKillcount");
                }
                else
                {
                    plrSound.Play();
                    GameObject ex = Instantiate(exBlue, hit.collider.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    ex.transform.localScale = Vector3.one * 1.5f;
                }
                Destroy(hit.collider.gameObject);
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.tag == "turret")
            {
                if (gameObject.name == "Player shot(Clone)") hit.collider.gameObject.SendMessage("GainAmmo");
                else
                {
                    turSound.Play();
                    GameObject ex = Instantiate(explosion, hit.collider.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    Destroy(hit.collider.gameObject);
                }
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.tag == "world") Destroy(gameObject);
        }
        if (distTravelled + (speed * Time.deltaTime) > maxRange) Destroy(gameObject);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        distTravelled += speed * Time.deltaTime;
    }
}
