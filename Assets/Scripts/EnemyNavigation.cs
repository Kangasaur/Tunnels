using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigation : MonoBehaviour
{
    float baseSpeed;
    float reloadTime;
    float visTime = 0f;
    GameObject player;
    public GameObject shot, shotSoundSource;
    Vector3 playerPos;
    RaycastHit hit;
    bool outside = true;
    GameObject navgoal_pri;
    GameObject in_navgoal_pri;
    List<GameObject> travelled = new List<GameObject>();
    Vector3 targetPos;
    LayerMask layerMask;
    int backtrack = 1;
    AudioSource shotSound, existSound;
    float soundInterval;
    float soundTime = 0f;

    void InitializeValues(object[] args)
    {
        player = (GameObject)args[0];
        baseSpeed = (float)args[1];
        soundInterval = 10f / baseSpeed;
    }

    GameObject CheckForTurrets()
    {
        foreach (GameObject turret in GameObject.FindGameObjectsWithTag("turret"))
        {
            if (Physics.Raycast(transform.position, turret.transform.position - transform.position, out hit, Mathf.Infinity, layerMask) && hit.collider.gameObject == turret)
            {
                return turret;
            }
        }
        return null;
    }

    void Start()
    {
        reloadTime = Random.Range(0.2f, 3f);
        layerMask = LayerMask.GetMask("ShipsAndWorld");
        shotSound = shotSoundSource.GetComponent<AudioSource>();
        existSound = GetComponent<AudioSource>();
        existSound.Play();
    }

    void Update()
    {
        soundTime += Time.deltaTime;
        if (soundTime >= soundInterval)
        {
            soundTime = 0f;
            existSound.Play();
        }

        if (player) playerPos = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, playerPos, out hit, Mathf.Infinity, layerMask) && hit.collider.gameObject == player)
        {
            transform.LookAt(player.transform.position);
            transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
            if (Vector3.Distance(transform.position, player.transform.position) > 1f) transform.Translate(Vector3.up * baseSpeed * Time.deltaTime);

            visTime += Time.deltaTime;
            if (visTime >= reloadTime)
            {
                visTime = 0f;
                reloadTime = Random.Range(2f, 3f);
                Instantiate(shot, transform.TransformPoint(Vector3.up * 0.5f), Quaternion.LookRotation(playerPos));
                shotSound.Play();
            }
        }
        else if (CheckForTurrets() != null)
        {
            GameObject turret = CheckForTurrets();
            transform.LookAt(turret.transform.position);
            transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
            transform.Translate(Vector3.up * baseSpeed * Time.deltaTime);

            visTime += Time.deltaTime;
            if (visTime >= reloadTime)
            {
                visTime = 0f;
                reloadTime = Random.Range(2f, 3f);
                Instantiate(shot, transform.TransformPoint(Vector3.up), Quaternion.LookRotation(turret.transform.position - transform.position));
                shotSound.Play();
            }
        }
        else if (outside)
        {
            if (navgoal_pri != null && Vector3.Distance(transform.position, navgoal_pri.transform.position) > baseSpeed * Time.deltaTime)
            {
                transform.LookAt(navgoal_pri.transform.position);
                transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
                transform.Translate(Vector3.up * baseSpeed * Time.deltaTime);
            }
            else
            {
                if (navgoal_pri != null)
                {
                    travelled.Add(navgoal_pri);
                    if (navgoal_pri.tag == "nav_agent_2")
                    {
                        transform.position = navgoal_pri.transform.position;
                        transform.LookAt(navgoal_pri.transform.Find("Turn(Clone)"));
                        transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
                        outside = false;
                        in_navgoal_pri = navgoal_pri;
                        navgoal_pri = null;
                        while (travelled.Count > 10) travelled.RemoveAt(0);
                    }
                }
                GameObject[] tunnelEntrances = GameObject.FindGameObjectsWithTag("nav_agent_2");
                if (tunnelEntrances.Length > 0)
                {
                    navgoal_pri = null;
                    GameObject navgoal = null;
                    float minDist = Mathf.Infinity;
                    foreach (GameObject tunnelEntrance in tunnelEntrances)
                    {
                        if (Physics.Raycast(transform.position, tunnelEntrance.transform.position - transform.position, out hit) && hit.collider.gameObject == tunnelEntrance && 
                            Vector3.Distance(transform.position, tunnelEntrance.transform.position) < minDist && !travelled.Contains(tunnelEntrance))
                        {
                            minDist = Vector3.Distance(transform.position, tunnelEntrance.transform.position);
                            navgoal_pri = tunnelEntrance;
                        }
                    }
                    if (outside && navgoal_pri == null && GameObject.FindGameObjectsWithTag("nav_agent_1").Length > 0)
                    {
                        foreach (GameObject nearBeacon in GameObject.FindGameObjectsWithTag("nav_agent_1"))
                        {
                            if (Physics.Raycast(transform.position, nearBeacon.transform.position - transform.position, out hit) && hit.collider.gameObject == nearBeacon && 
                                Vector3.Distance(transform.position, nearBeacon.transform.position) < minDist && !travelled.Contains(nearBeacon))
                            {
                                minDist = Vector3.Distance(transform.position, nearBeacon.transform.position);
                                navgoal_pri = navgoal;
                                navgoal = nearBeacon;
                            }
                        }
                        if (navgoal_pri == null) navgoal_pri = navgoal;
                    }
                    if (navgoal_pri == null && GameObject.FindGameObjectsWithTag("nav_agent_0").Length > 0)
                    {
                        foreach (GameObject beacon in GameObject.FindGameObjectsWithTag("nav_agent_0"))
                        {
                            if (Physics.Raycast(transform.position, beacon.transform.position - transform.position, out hit) && hit.collider.gameObject == beacon &&
                                Vector3.Distance(transform.position, beacon.transform.position) < minDist && !travelled.Contains(beacon))
                            {
                                minDist = Vector3.Distance(transform.position, beacon.transform.position);
                                navgoal_pri = navgoal;
                                navgoal = beacon;
                            }
                        }
                        if (navgoal_pri == null) navgoal_pri = navgoal;
                    }
                }
            }
        }
        else
        {
            if (in_navgoal_pri != null && Vector3.Distance(transform.position, in_navgoal_pri.transform.position) > baseSpeed * Time.deltaTime)
            {
                transform.LookAt(in_navgoal_pri.transform.position);
                transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y - 180, transform.rotation.eulerAngles.z);
                transform.Translate(Vector3.up * baseSpeed * Time.deltaTime);
            }
            else
            {
                if (in_navgoal_pri != null)
                {
                    travelled.Add(in_navgoal_pri);
                    if (travelled.Count > 10) travelled.RemoveAt(0);
                    if (Random.Range(0f, 100f) > 70) transform.rotation = Quaternion.Euler(-in_navgoal_pri.transform.rotation.eulerAngles.x - 90, in_navgoal_pri.transform.rotation.eulerAngles.y - 180, 
                        in_navgoal_pri.transform.rotation.eulerAngles.z);
                    if (in_navgoal_pri.name == "Tunnel entrance(Clone)")
                    {
                        outside = true;
                        in_navgoal_pri = null;
                    }
                }
                GameObject[] tunnelTurns = GameObject.FindGameObjectsWithTag("nav_agent_2");
                if (tunnelTurns.Length > 0)
                {
                    in_navgoal_pri = null;
                    float maxDot = -1f;
                    foreach (GameObject tunnelTurn in tunnelTurns)
                    {
                        if (Physics.Raycast(transform.position, tunnelTurn.transform.position - transform.position, out hit) && hit.collider.gameObject == tunnelTurn &&
                            Vector3.Dot(transform.TransformDirection(Vector3.up), (tunnelTurn.transform.position - transform.position).normalized) > maxDot && !travelled.Contains(tunnelTurn))
                        {
                            maxDot = Vector3.Dot(transform.TransformDirection(Vector3.up), (tunnelTurn.transform.position - transform.position).normalized);
                            in_navgoal_pri = tunnelTurn;
                            backtrack = 1;
                        }
                    }
                    while (in_navgoal_pri == null && backtrack < 10)
                    {
                        backtrack++;
                        GameObject backtarget = travelled[travelled.Count - backtrack];
                        if (Physics.Raycast(transform.position, backtarget.transform.position - transform.position, out hit) && hit.collider.gameObject == backtarget)
                        {
                            in_navgoal_pri = backtarget;
                        }
                    }
                }
            }
        }
    }
}
