using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour
{
    float rotspeed = 90.0f;
    float speed = 2.0f;
    float hRot, vRot;
    public GameObject cubeMaker, tunnelBeacon, turn, spawner, shot, firstSpawner, mine, dBomb, turret;
    List<GameObject> toDetonate = new List<GameObject>();
    GameObject beacon;
    bool initiated = false;
    bool outside = true;
    bool turning = false;
    float turnCountdown = 0f;
    int turnCount = 0;
    public static float cooldownTime = 45f;
    float dTime = 0f;
    bool detonated = false;

    public GameObject digSoundSource, shootSoundSource, mineSoundSource, dBombSoundSource, turretSoundSource, musicController;
    AudioSource digSound, shootSound, mineSound, dBombSound, turretSound;

    void Start()
    {
        transform.position = Vector3.one * 100;
        transform.rotation = Quaternion.Euler(-90f, Random.Range(0f, 360f), Random.Range(-70f, 70f));
        transform.Translate(Vector3.right * 150);
        firstSpawner.SendMessage("SetUpShips", gameObject);
        digSound = digSoundSource.GetComponent<AudioSource>();
        shootSound = shootSoundSource.GetComponent<AudioSource>();
        mineSound = mineSoundSource.GetComponent<AudioSource>();
        dBombSound = dBombSoundSource.GetComponent<AudioSource>();
        turretSound = turretSoundSource.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!initiated)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out RaycastHit hit))
            {
                transform.position = hit.point;
                transform.Translate(Vector3.right * 10);
            }
            initiated = true;
        }

        if (cooldownTime < 120f) cooldownTime += Time.deltaTime;

        hRot = Input.GetAxis("Horizontal") * rotspeed * Time.deltaTime;
        vRot = Input.GetAxis("Vertical") * rotspeed * Time.deltaTime;
        transform.Rotate(Vector3.up, vRot);
        transform.Rotate(Vector3.forward, hRot);
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && cooldownTime >= 0.75f && !PauseMenu.isPaused)
        {
            cooldownTime -= 0.75f;
            Instantiate(shot, transform.TransformPoint(Vector3.left * 0.5f), Quaternion.LookRotation(transform.TransformDirection(Vector3.left)));
            shootSound.Play();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1) && cooldownTime >= 45f && !PauseMenu.isPaused)
        {
            cooldownTime -= 45f;
            Instantiate(mine, transform.TransformPoint(Vector3.right), Quaternion.LookRotation(transform.TransformDirection(Vector3.left)));
            mineSound.Play();
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (dTime < 2f) dTime += Time.deltaTime;
            else
            {
                foreach (GameObject detonate in toDetonate)
                {
                    detonate.SendMessage("Detonate");
                }
                detonated = true;
                toDetonate = new List<GameObject>();
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha2) && !PauseMenu.isPaused)
        {
            if (!detonated)
            {
                if (cooldownTime >= 50f)
                {
                    cooldownTime -= 50f;
                    GameObject detonate = Instantiate(dBomb, transform.TransformPoint(Vector3.right), Quaternion.LookRotation(transform.TransformDirection(Vector3.forward)));
                    toDetonate.Add(detonate);
                    dBombSound.Play();
                }
            }
            else detonated = false;
            dTime = 0f;
        }

        if (Input.GetKeyUp(KeyCode.Alpha3) && cooldownTime >= 120f && !PauseMenu.isPaused)
        {
            cooldownTime -= 120f;
            Instantiate(turret, transform.TransformPoint(Vector3.right), Quaternion.LookRotation(transform.TransformDirection(Vector3.right)));
            turretSound.Play();
        }

        if (!outside)
        {
            if (!turning)
            {
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    turning = true;
                    Instantiate(turn, transform.position, transform.rotation, beacon.transform);
                }
            }
            else
            {
                if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                {
                    turning = false;
                    Instantiate(turn, transform.position, transform.rotation, beacon.transform);
                    turnCountdown = 0f;
                    turnCount = 0;
                }
                if (turnCountdown > 0.5f && turnCount < 5)
                {
                    turnCountdown = 0f;
                    Instantiate(turn, transform.position, transform.rotation, beacon.transform);
                    turnCount++;
                }
                turnCountdown += Time.deltaTime;
            }
            if (Physics.OverlapSphere(transform.position, 1f).Length <= 1)
            {
                outside = true;
                Instantiate(tunnelBeacon, transform.position, transform.rotation);
                GameObject newSpawner = Instantiate(spawner, transform.position, transform.rotation);
                newSpawner.transform.Translate(Vector3.left * 30f);
                newSpawner.SendMessage("SetUpShips", gameObject);
                firstSpawner.SetActive(true);
                firstSpawner.SendMessage("DoOutsideSpawn");
                musicController.SendMessage("SetOutside", true);
            }
        }

        foreach (Collider collider in Physics.OverlapSphere(transform.position, 0.4f))
        {
            if (collider.gameObject.tag == "world")
            {
                cubeMaker.SendMessage("DeleteVoxel", collider.gameObject);
                if (outside)
                {
                    outside = false;
                    beacon = Instantiate(tunnelBeacon, transform.position, transform.rotation);
                    GameObject newSpawner = Instantiate(spawner, transform.position, transform.rotation);
                    newSpawner.transform.Translate(Vector3.right * 30f);
                    newSpawner.SendMessage("SetUpShips", gameObject);
                    firstSpawner.SetActive(false);
                    musicController.SendMessage("SetOutside", false);
                }
                digSound.Play();
            }
        }
    }

    void OnDestroy()
    {
        cooldownTime = 45f;
        GameObject.Find("Canvas").SendMessage("HandleDeath");
    }
}
