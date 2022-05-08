using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kev_TimeAttackLevelManager : MonoBehaviour
{
    public GameObject gameoverScreen;
    public float elapsedTime = 0f;
    public float remainingTime = 3f;
    public bool timePaused = false;

    public TMPro.TextMeshProUGUI remainingText;

    // Start is called before the first frame update
    void Start()
    {
        gameoverScreen.SetActive(false);
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timePaused == false)
        {
            elapsedTime = elapsedTime + Time.deltaTime;
            remainingTime = Mathf.Clamp(remainingTime - Time.deltaTime, 0f, float.PositiveInfinity);
        }
        remainingText.text = $"time: {remainingTime:F1}s";

        if (remainingTime <= 0f)
        {
            GameOver();
        }

    }
    void GameOver()
    {
        gameoverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
