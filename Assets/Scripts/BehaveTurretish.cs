using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaveTurretish : MonoBehaviour
{
    float reloadTime = 0.75f;
    float visTime = 0f;
    RaycastHit hit;
    LayerMask layerMask;
    GameObject target;
    public GameObject shot;
    int shotsNum = 3;
    AudioSource shootSound;

    GameObject CheckForEnemies()
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("ship"))
        {
            if (Physics.Raycast(transform.position, ship.transform.position - transform.position, out hit, Mathf.Infinity, layerMask) && hit.collider.gameObject == ship && ship.name == "ship2(Clone)")
            {
                return ship;
            }
        }
        return null;
    }

    private void Start()
    {
        layerMask = LayerMask.GetMask("ShipsAndWorld");
        shootSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (CheckForEnemies() != null)
        {
            target = CheckForEnemies();
            transform.LookAt(target.transform.position);
            visTime += Time.deltaTime;
            if (visTime >= reloadTime && shotsNum > 0)
            {
                visTime = 0f;
                Instantiate(shot, transform.TransformPoint(Vector3.forward * 0.5f), Quaternion.LookRotation(target.transform.position - transform.position));
                shootSound.Play();
                shotsNum--;
            }
        }
    }

    void GainAmmo()
    {
        shotsNum++;
        Debug.Log(shotsNum);
    }
}
