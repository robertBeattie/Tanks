using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStartOfMission : MonoBehaviour
{
    public UIManager uiManager;
    public LevelSelect levelSelect;

    public GameObject duringPlay;
    public GameObject startUI;

    public GameObject beforePlay;
    public RawImage backGround;
    public GameObject bonusTank;
    public GameObject mission;
    public TextMeshProUGUI missionCount;
    public TextMeshProUGUI enemyTankCount;
    public TextMeshProUGUI playerTankCount;
    public GameObject startui;
    public GameObject countdownui;
    public TextMeshProUGUI countdownText;
    bool backgroundScroll = false;
    float backX = 0;
    float backY = 0;
    public GameObject howTo;
    public GameObject controllButtons;
    [SerializeField]
    bool isJoystick = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (backgroundScroll)
        {
            

        }
        float WIDTH = backGround.uvRect.width;
        float HEIGHT = backGround.uvRect.height;
        backX -= 1f * Time.deltaTime;
        backY -= 1f * Time.deltaTime;
        //backGround.uvRect.Set(backX, backY, WIDTH, HEIGHT);
        backGround.uvRect = new Rect(backX, backY, WIDTH, HEIGHT);
    }
    
    public IEnumerator StartUI()
    {
        //showBeforePlay
        beforePlay.SetActive(true);
        //start Scrolling background
        ScrollBackground(true);
        //Set player tank count
        playerTankCount.SetText(uiManager.p1.getDeathsText());
        //if level mod 5 bonus tank
        if ((levelSelect.levelNum() % 5) == 0 &&
            levelSelect.levelNum() != 0 &&
            uiManager.p1.getLastSceneNum() != levelSelect.levelNum())
        {
            //show bonus tank
            bonusTank.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            //increase player tank count here
            uiManager.setLives(uiManager.p1.deaths + 1);
            playerTankCount.SetText(uiManager.p1.getDeathsText());
            //show for 3 sec
            yield return new WaitForSeconds(1.5f);
            //hide bonus tank
            bonusTank.SetActive(false);
        }


        //show mission
        mission.SetActive(true);
        //Set Mission count
        missionCount.SetText(levelSelect.levelNum().ToString());
        //Set Enemy tank count
        //enemyTankCount.SetText(uiManager.getTanksLeft().ToString());
        //show for 5 sec
        yield return new WaitForSeconds(3f);
        //hide mission
        mission.SetActive(false);
        //hideBeforePlay
        beforePlay.SetActive(false);
        //stop Scrolling background
        ScrollBackground(false);

        duringPlay.SetActive(false);
        //3
        countdownui.SetActive(true);
        countdownText.SetText("3");
        yield return new WaitForSeconds(1f);
        
        //2
        countdownText.SetText("2");
        yield return new WaitForSeconds(1f);
        //1
        countdownText.SetText("1");
        yield return new WaitForSeconds(1f);
        duringPlay.SetActive(true);
        if (!isJoystick)
        {
            controllButtons.SetActive(false);
        }
        if (levelSelect.levelNum() == 1)
        {
            howTo.SetActive(true);
        }
        else
        {
            howTo.SetActive(false);
        }
        countdownui.SetActive(false);
        //start
        uiManager.TimePause();
        uiManager.pauseLock = false;
        startui.SetActive(true);
        yield return new WaitForSeconds(1f);
        startui.SetActive(false);
    }
    void ScrollBackground(bool active)
    {
        backgroundScroll = active;
    }

}
