using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public TextMeshProUGUI missionNumberText;
    public TextMeshProUGUI tanksLeftText;
    public Score p1;
    public TextMeshProUGUI playerLives;
    LevelSelect levelSelect;
    [SerializeField]
    int tanksLeft = 0;

    int startScore;
    public bool pauseLock = true;
    bool pause = false;
    //removing tanks already defeated
    List<string> deadTanks;

    //pause button
    public Button pauseButton;
    public Sprite pauseSprite;
    public Sprite playSprite;

    //StartUI
    public UIStartOfMission uIStartOfMission;

    public GameObject MissionCleared;

    // Start is called before the first frame update
    void Start()
    {
        levelSelect = this.gameObject.GetComponent<LevelSelect>();
        updateMissionNumber(levelSelect.levelNum());
        updateP1Score();
        updateP2Score(0);
        updateLives();
        startScore = p1.getPoints();
        //StartGameUI
        StartCoroutine(StartLevel());
        //removeOldTanks();
        p1.setLastSceneNum(levelSelect.levelNum());
        mineButtonSetUp();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseLock)
        {
            TimePause();
        }
    }
    public void updateP1Score()
    {
        p1ScoreText.SetText(p1.getPointsText());
    }
    public void addP1Score(int num)
    {
        p1.addPoints(num);
        updateP1Score();
    }
    public void setP1Score(int num)
    {
        p1.setPoints(num);
        updateP1Score();
    }
    public void updateP2Score(int num)
    {
        p2ScoreText.SetText(num.ToString());
    }
    public void addP2Score(int num)
    {
        updateP2Score(Int32.Parse(p2ScoreText.text) + num);
    }
    public void updateMissionNumber(int num)
    {
        missionNumberText.SetText(num.ToString());
    }
    public void updateTanksLeft(int num)
    {
        tanksLeftText.SetText(num.ToString());
        uIStartOfMission.enemyTankCount.SetText(num.ToString());
        //mission complete

        if (tanksLeft <= 0) {
            //next level levelSelect.nextLevel();
            StartCoroutine(NextLevel());
        }
    }
    public void removeTanksLeft(int num)
    {
        tanksLeft--;
        updateTanksLeft(tanksLeft);
    }
    public void addTanksLeft(int num)
    {
        tanksLeft++;
        updateTanksLeft(tanksLeft);
       
    }
    public int getTanksLeft()
    {
        return tanksLeft;
    }
    public void updateLives()
    {
        playerLives.SetText(p1.getDeathsText());
        if(p1.getDeaths() <= 0)
        {
            levelSelect.LoseGame();
        }
    }
    public void removeLives(int num)
    {
        p1.removeDeaths(num);
        updateLives();
    }
    public void setLives(int num)
    {
        p1.setDeaths(num);
        updateLives();
    }
    public void resetScore()
    {
        //p1.setPoints(startScore);
    }

    IEnumerator NextLevel()
    {
        TimePause();
        //MissionCleared UI and 
        MissionCleared.SetActive(true);
        //Destroyed UI count
        yield return new WaitForSeconds(1.3f);
        MissionCleared.SetActive(false);
        //TimePause();
        levelSelect.nextLevel();
    }
    public void TimePause()
    {
        pause = !pause;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<Bullet>().Pause(pause);
        }
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Enemy>().Pause(pause);
        }
        foreach (GameObject player in players)
        {
            player.GetComponent<Controller>().Pause(pause);
        }

    }
    IEnumerator StartLevel()
    {
        TimePause();
        StartCoroutine(uIStartOfMission.StartUI());
        yield return new WaitForSeconds(0.1f);
    }
    private void removeOldTanks()
    {
        if (p1.getLastSceneNum() == levelSelect.levelNum())
        {
            deadTanks = p1.getDeadTanks();
            foreach(string deadTank in deadTanks)
            {
                GameObject tank = GameObject.Find(deadTank);
                Debug.Log(tank);
                tank.GetComponent<EnemyDeath>().deathWithOutEffect();
            }
        }
    }
    public void addDeadTank(string deadTank)
    {
        p1.addDeadTank(deadTank);
    }
    public bool findDeadTank(string name)
    {
       return p1.findDeadTank(name);
    }
    public void mineButtonSetUp()
    {
        GameObject button = GameObject.Find("LandMine Button");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        button.GetComponent<Button>().onClick.AddListener(delegate {
            player.GetComponent<Controller>().LayMine(); });
    }
    public void PauseButton()
    {
        TimePause();
        if (!pause)
        {
            pauseButton.gameObject.GetComponent<Image>().sprite = pauseSprite;
        }
        else
        {
            pauseButton.gameObject.GetComponent<Image>().sprite = playSprite;
        }
    }
    
}
