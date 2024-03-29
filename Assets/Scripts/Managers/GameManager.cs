﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private void OnEnable()
	{
        Door.OnUnlock += GotoNextLevel;
	}

    private void OnDisable()
    {
        Door.OnUnlock -= GotoNextLevel;
    }

    public void GotoNextLevel()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextScene + 1 > sceneCount)
        {
            //GameOver();
            Debug.Log("Game Over");
            SceneManager.LoadScene(0);
            return;
        }

        SceneManager.LoadScene(nextScene);
    }

    public void GameOver()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
