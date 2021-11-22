using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject explotionPreFab;
    public GameObject xMarkerPrefab;
    public GameObject piviotBottom;
    public GameObject piviotTop;
    public Component[] deActivateList;
    UIManager uiManager;
    bool dead = false;
    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        if (uiManager.findDeadTank(gameObject.name)){
            deathWithOutEffect();
            return;
        }
        uiManager.addTanksLeft(1);
    }
    public void death()
    {
        if (dead){ return; };
        dead = true;
        GameObject explotion = Instantiate(explotionPreFab, this.transform.position, this.transform.rotation);
        GameObject xMarker = Instantiate(xMarkerPrefab, this.transform.position, this.transform.rotation);
        uiManager.addP1Score(1);
        uiManager.addDeadTank(this.gameObject.name);
        uiManager.removeTanksLeft(1);
        piviotTop.SetActive(false);
        piviotBottom.SetActive(false);
        foreach (Component component in deActivateList)
        {
            DisableComponent(component);
        }

    }
    public void deathWithOutEffect()
    {
        if (dead) { return; };
        dead = true;
        piviotTop.SetActive(false);
        piviotBottom.SetActive(false);
        foreach (Component component in deActivateList)
        {
            DisableComponent(component);
        }

    }
    public static void DisableComponent(Component component)
    {
        if (component is Renderer)
        {
            (component as Renderer).enabled = false;
        }
        else if (component is Collider)
        {
            (component as Collider).enabled = false;
        }
        else if (component is Behaviour)
        {
            (component as Behaviour).enabled = false;
        }
    }
}