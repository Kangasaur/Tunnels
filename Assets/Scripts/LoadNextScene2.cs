using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene2 : MonoBehaviour
{
    bool loading = false;

    void Update()
    {
        if (!loading)
        {
            loading = true;
            SceneManager.LoadScene(4);
        }
    }
}
