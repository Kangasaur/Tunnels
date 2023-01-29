using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateVoxels : MonoBehaviour
{
    public static int[,,] voxels;
    int height = 200, width = 200, length = 200;
    RaycastHit hit;
    public Material tunnel;
    public GameObject ship;

    void Start()
    {
        voxels = new int[length, height, width];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    voxels[x, y, z] = 0;
                }
            }
        }
        voxels = AsteroidGrid(voxels, length, height, width);
        for (int x = 1; x < length - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                for (int z = 1; z < width - 1; z++)
                {
                    if (voxels[x, y, z] == 1)
                    {
                        if (voxels[x-1, y, z] == 0 || voxels[x+1, y, z] == 0 || 
                            voxels[x, y-1, z] == 0 || voxels[x, y+1, z] == 0 ||
                            voxels[x, y, z-1] == 0 || voxels[x, y, z+1] == 0)
                        {
                            int voxType = GetNeighborIndex(x, y, z, voxels);
                            GameObject Voxel = Resources.Load<GameObject>(string.Format("Voxel_Types/Voxel_{0}", voxType));
                            Instantiate(Voxel, new Vector3(x, y, z), Voxel.transform.rotation, transform);
                        }
                    }
                }
            }
        }
    }

    void DeleteVoxel(GameObject cube)
    {
        Vector3 pos = cube.transform.position;
        voxels[(int)pos.x, (int)pos.y, (int)pos.z] = 0;
        Destroy(cube);
        Vector3[] voxcheck = { pos + Vector3.up, pos + Vector3.down, pos + Vector3.forward, pos + Vector3.back, pos + Vector3.left, pos + Vector3.right };
        foreach (Vector3 check in voxcheck)
        {
            if (voxels[(int)check.x, (int)check.y, (int)check.z] == 1)
            {
                foreach (Collider collider in Physics.OverlapSphere(check, 0.25f))
                {
                    if (collider.gameObject.tag == "world") Destroy(collider.gameObject);
                }
                int voxType = GetNeighborIndex((int)check.x, (int)check.y, (int)check.z, voxels);
                GameObject Voxel = Resources.Load<GameObject>(string.Format("Voxel_Types/Voxel_{0}", voxType));
                GameObject v = Instantiate(Voxel, check, Voxel.transform.rotation, transform);
                v.GetComponent<Renderer>().material = tunnel;
            }
        }
    }

    int GetNeighborIndex(int x, int y, int z, int[,,] world)
    {
        int nIndex = 0;
        if (world[x, y + 1, z] == 1) nIndex += 1; //Top
        if (world[x - 1, y, z] == 1) nIndex += 2; //Left
        if (world[x, y, z + 1] == 1) nIndex += 4; //Front
        if (world[x + 1, y, z] == 1) nIndex += 8; //Right
        if (world[x, y, z - 1] == 1) nIndex += 16; //Back
        if (world[x, y - 1, z] == 1) nIndex += 32; //Bottom
        return nIndex;
    }

    int[,,] AsteroidGrid(int[,,] grid, int length, int height, int width)
    {
        float percent = 0f;
        while (percent <= 2.4f || percent >= 9f)
        {
            percent = 0f;
            float xCoord = Random.Range(0, 100000f);
            float yCoord = Random.Range(0, 100000f);
            float zCoord = Random.Range(0, 100000f);
            for (int x = 1; x < length - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    for (int z = 1; z < width - 1; z++)
                    {
                        Vector3 newXYZ = new Vector3(100 - (100 - x)/2, 100 - (100 - y)/2, 100 - (100 - z)/2);
                        float distReciprocal = 2.5f / Vector3.Distance(newXYZ, Vector3.one * 100);
                        if (GenerateAsteroid(xCoord + newXYZ.x, yCoord + newXYZ.y, zCoord + newXYZ.z, 512) + distReciprocal > 0.355f)
                        {
                            grid[x, y, z] = 1;
                            percent += 1;
                        }
                        else grid[x, y, z] = 0;
                    }
                }
            }
            percent = percent / (length * width * height) * 100;
        }
        return grid;
    }

    float GenerateAsteroid(float x, float y, float z, float scale)
    {
        float p1 = Perlin3D(x / scale, y / scale, z / scale);
        float p2 = Perlin3D(x / scale * 2, y / scale * 2, z / scale * 2);
        float p3 = Perlin3D(x / scale * 4, y / scale * 4, z / scale * 4);
        float p4 = Perlin3D(x / scale * 8, y / scale * 8, z / scale * 8);
        float p5 = Perlin3D(x / scale * 16, y / scale * 16, z / scale * 16);
        return Mathf.Pow((p1 + p2 + p3 + p4 + p5) / 5, 2f);
    }

    float Perlin3D(float x, float y, float z)
    {
        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);

        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        return (AB + BC + AC + BA + CB + CA) / 6;
    }
}
