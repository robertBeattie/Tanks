using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIScore : MonoBehaviour
{
    LevelSelect level;
    public TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        level = GetComponent<LevelSelect>();
        score.SetText(level.playerScore.getPointsText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
