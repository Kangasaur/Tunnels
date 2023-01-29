using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    bool loading = false;
    string[] tooltips = new string[] { "TIP: Being able to see what you're doing is useful! Don't forget to use the camera controls: Q, E, and the mouse scrollwheel to zoom.",
    "TIP: If you aren't having a good day, try taking five minutes to just do nothing. Relax, close your eyes, maybe listen to your favorite song, and come back to your job with a clear head.",
    "TIP: Be careful not to stumble into one of your own mines. (Or near a detonating D-Bomb!)", "TIP: Staying outside for too long is dangerous. Be careful!",
    "TIP: Pressing and holding 2 will set off all of the D-Bombs in the scene.", "TIP: Turrets start with 3 shots, but you can give them more if you shoot at them."};
    public GameObject tipText;

    void Start()
    {
        tipText.GetComponent<TMPro.TextMeshProUGUI>().text = tooltips[Random.Range((int)0, tooltips.Length)];
    }

    void Update()
    {
        if (!loading)
        {
            loading = true;
            SceneManager.LoadScene(4);
        }
    }
}
