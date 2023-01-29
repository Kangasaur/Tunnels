using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public static bool isDead = false;
    public GameObject pauseOverlay, deathOverlay, deathText1, deathText2, mineHUD, dBombHUD, turretHUD, pauseSound;
    public Color fullColor, defaultColor;
    Slider mineSlider, dBombSlider, turretSlider;
    Image mineSliderColor, dBombSliderColor, turretSliderColor;
    AudioSource pauseAudio;
    int killcount = 0;
    float time = 0f;

    void Start()
    {
        mineSlider = mineHUD.GetComponent<Slider>();
        mineSliderColor = mineHUD.GetComponentInChildren<Image>();
        dBombSlider = dBombHUD.GetComponent<Slider>();
        dBombSliderColor = dBombHUD.GetComponentInChildren<Image>();
        turretSlider = turretHUD.GetComponent<Slider>();
        turretSliderColor = turretHUD.GetComponentInChildren<Image>();
        pauseAudio = pauseSound.GetComponent<AudioSource>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
        {
            if (isPaused) Resume();
            else Pause();
        }
        else if (isDead && deathOverlay.GetComponent<Image>().color.a > 0f && Input.GetKeyDown(KeyCode.Escape))
        {
            isDead = false;
            time = 0f;
            killcount = 0;
            ExitToMainMenu();
        }
        else if (isDead && deathOverlay.GetComponent<Image>().color.a > 0f && Input.GetKeyDown(KeyCode.Space))
        {
            isDead = false;
            time = 0f;
            killcount = 0;
            Restart();
        }

        if (!isDead)
        {
            mineSlider.value = Mathf.Clamp(MoveShip.cooldownTime / 45f, 0f, 1f);
            if (mineSlider.value == 1f) mineSliderColor.color = fullColor;
            else mineSliderColor.color = defaultColor;
            dBombSlider.value = Mathf.Clamp(MoveShip.cooldownTime / 50f, 0f, 1f);
            if (dBombSlider.value == 1f) dBombSliderColor.color = fullColor;
            else dBombSliderColor.color = defaultColor;
            turretSlider.value = Mathf.Clamp(MoveShip.cooldownTime / 120f, 0f, 1f);
            if (turretSlider.value == 1f) turretSliderColor.color = fullColor;
            else turretSliderColor.color = defaultColor;
        }
    }

    public void Resume()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        pauseAudio.Play();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(3);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(2);
    }

    void HandleDeath()
    {
        isDead = true;
        deathOverlay.SetActive(true);
        Image image = deathOverlay.GetComponent<Image>();
        TMPro.TextMeshProUGUI text = deathText1.GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI text2 = deathText2.GetComponent<TMPro.TextMeshProUGUI>();
        string pursuers = " pursuers";
        string seconds = " seconds";
        if ((int)time == 1) seconds = " second";
        if (killcount == 1) pursuers = " pursuer";
        text2.text = "Lasted " + (int)time + seconds + "\nDestroyed " + killcount + pursuers;
        Color newcolor = image.color;
        newcolor.a = 0f;
        image.color = newcolor;
        newcolor = text.color;
        newcolor.a = 0f;
        text.color = newcolor;
        text2.color = newcolor;
        StartCoroutine(LoadDeathScreen(image, text, text2));
    }

    void IncrementKillcount()
    {
        killcount++;
    }

    IEnumerator LoadDeathScreen(Image image, TMPro.TextMeshProUGUI text, TMPro.TextMeshProUGUI text2)
    {
        float countdown = 0f;
        float a = 0f;
        while (a < 1f)
        {
            countdown += Time.deltaTime;
            if (countdown > 2f)
            {
                a += 0.5f * Time.deltaTime;
                Color newcolor = image.color;
                newcolor.a = a / 2.5f;
                image.color = newcolor;
                newcolor = text.color;
                newcolor.a = a;
                text.color = newcolor;
                text2.color = newcolor;
            }
            yield return null;
        }
        yield return null;
    }
}
