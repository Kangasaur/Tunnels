using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    GameObject target, ui;
    float speed = 20f;
    
    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < speed * Time.deltaTime)
            {
                transform.position = target.transform.position;
                ui.SetActive(true);
                foreach (GameObject textbox in GameObject.FindGameObjectsWithTag("text"))
                {
                    if (textbox.transform.IsChildOf(ui.transform))
                    {
                        TMPro.TextMeshProUGUI text = textbox.GetComponent<TMPro.TextMeshProUGUI>();
                        Color newColor = text.color;
                        newColor.a = 0;
                        text.color = newColor;
                        StartCoroutine(Fade(text));
                    }
                }
                target = null;
                ui = null;
            }
            else transform.Translate((target.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
    }

    public void GoToMenu(GameObject menuloc)
    {
        target = menuloc;
    }

    public void SetMenu(GameObject menu)
    {
        ui = menu;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayGame2()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator Fade(TMPro.TextMeshProUGUI text)
    {
        while (text.color.a < 1)
        {
            Color newColor = text.color;
            newColor.a += 2f * Time.deltaTime;
            text.color = newColor;
            yield return null;
        }
        yield return null;
    }
}
