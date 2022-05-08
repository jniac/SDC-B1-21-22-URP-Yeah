using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class qng_TimeAttackLevelManager : MonoBehaviour
{
    public GameObject gameoverScreen; 
    public float elapsedTime = 0f;
    public float remainingTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        gameoverScreen.SetActive(false);
        elapsedTime = 0f; 
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime; 
        remainingTime -= Time.deltaTime;
        
        if (remainingTime < 0f) 
        {
            GameOver();
        }
    }

    void GameOver()
    { 
        gameoverScreen.SetActive(true);
        Time.timeScale=0f; 
    }
}


