using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Score", order = 1)]
public class Score : ScriptableObject
{
    public int points;
    public int deaths = 3;
    [SerializeField]
    int lastScene;
    [SerializeField]
    List<string> deadTanks = new List<string>();

    //Points-------------------------------------
    public void setPoints(int points)
    {
        this.points = points;
    }
    public void addPoints(int points)
    {
        this.points += points;
    }
    public void removePoints(int points)
    {
        this.points -= points;
    }
    public int getPoints()
    {
        return points;
    }
    public string getPointsText()
    {
        return points.ToString();
    }
    //Deaths-------------------------------------
    public void setDeaths(int deaths)
    {
        this.deaths = deaths;
    }
    public void addDeaths(int deaths)
    {
        this.deaths += deaths;
    }
    public void removeDeaths(int deaths)
    {
        this.deaths -= deaths;
    }
    public int getDeaths()
    {
        return deaths;
    }
    public string getDeathsText()
    {
        return deaths.ToString();
    }
    public void addDeadTank(string deadTank)
    {
        deadTanks.Add(deadTank);
    }
    public void resetDeadTanks()
    {
        deadTanks.Clear();
    }
    public List<string> getDeadTanks()
    {
        return deadTanks;
    }
    public bool findDeadTank(string name)
    {
        return deadTanks.Contains(name);
    }
    public int getLastSceneNum()
    {
        return lastScene;
    }
    public void setLastSceneNum(int x)
    {
        lastScene = x;
    }
}
