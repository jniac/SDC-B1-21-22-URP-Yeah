using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nvt_TimeAttackLevelManager : MonoBehaviour
{
    public GameObject gameoverScreen;
    public float elapsedTime = 0f;
    public float remainingTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        gameoverScreen.SetActive(false);
        elapsedTime = 0f;
    }

    // Update is called once per frame
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
        gameoverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
