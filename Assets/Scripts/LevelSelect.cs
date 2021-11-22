using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public Score playerScore;
    public void nextLevel()
    {
        if(levelNum() == 0)
        {
            playerScore.setDeaths(3);
            playerScore.setPoints(0);
        }
        if ((levelNum() % 5) == 0 && levelNum() != 0)
        {
            playerScore.addDeaths(1);
        }
        if(levelNum() != SceneManager.sceneCountInBuildSettings)
        {
            playerScore.resetDeadTanks();
            SceneManager.LoadScene(levelNum() +1);
        }
        else
        {
            Debug.Log("Last Level");
        }
    }
        
    public int levelNum()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void restartGame()
    {
        playerScore.setDeaths(3);
        playerScore.setPoints(0);
        SceneManager.LoadScene(0);
        playerScore.resetDeadTanks();
    }
    public void LoseGame()
    {
        SceneManager.LoadScene(23);
    }
    public void menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Settings()
    {
        SceneManager.LoadScene(22);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
  
}
