using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShips : MonoBehaviour
{
    public GameObject ship, outsideMusic;
    GameObject player;
    float rate = 30f;
    float time = 0f;
    float speed = 3f;
    bool firstSpawn = false;
    
    void SetUpShips(GameObject initPlayer)
    {
        player = initPlayer;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > rate)
        {
            time = 0f;
            bool spawned = false;
            while (!spawned)
            {
                Vector3 spawnPos = transform.position + new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
                if (Physics.OverlapSphere(spawnPos, 1f).Length == 0 && InstantiateVoxels.voxels[(int)spawnPos.x, (int)spawnPos.y, (int)spawnPos.z] == 0)
                {
                    GameObject newShip = Instantiate(ship, spawnPos, transform.rotation);
                    newShip.SendMessage("InitializeValues", new object[] { player, speed });
                    spawned = true;
                    speed += 0.25f;
                }
            }
            if (!firstSpawn && gameObject.name == "First Spawner")
            {
                rate = 10f;
                outsideMusic.GetComponent<AudioSource>().Play();
                firstSpawn = true;
            }
        }
    }

    void DoOutsideSpawn()
    {
        if (!firstSpawn)
        {
            rate = 10f;
            outsideMusic.GetComponent<AudioSource>().Play();
            firstSpawn = true;
        }
    }
}
