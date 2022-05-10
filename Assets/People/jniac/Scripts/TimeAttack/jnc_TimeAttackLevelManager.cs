using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jnc_TimeAttackLevelManager : MonoBehaviour
{
    public GameObject gameoverScreen, winScreen;
    public float elapsedTime = 0f;
    public float remainingTime = 3f;
    public bool timePaused = false;

    void Start()
    {
        gameoverScreen.SetActive(false);
        winScreen.SetActive(false);
        elapsedTime = 0f;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (timePaused == false)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            remainingTime = Mathf.Clamp(remainingTime - Time.deltaTime, 0f, float.PositiveInfinity);
        }

        if (remainingTime <= 0f)
        {
            GameOver();
        }

    }

    public void Win()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        gameoverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
}
