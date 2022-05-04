using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unj_TimeAttackLevelManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public float elapsedTime = 0f;
    public float remainingTime = 3f;

    void Start()
    {
        gameOverScreen.SetActive(false);
        elapsedTime = 0f;

    }

    void Update()
    {
        elapsedTime = elapsedTime + Time.deltaTime;
        remainingTime = remainingTime - Time.deltaTime;

        if (remainingTime < 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
