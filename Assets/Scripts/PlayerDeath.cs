using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject explotionPreFab;
    public GameObject xMarkerPrefab;
    public GameObject piviotBottom;
    public GameObject piviotTop;
    public Component[] deActivateList;
    public UIManager uiManager;

    private void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void death()
    {
        uiManager.removeLives(1);
        GameObject explotion = Instantiate(explotionPreFab, this.transform.position, this.transform.rotation);
        GameObject xMarker = Instantiate(xMarkerPrefab, this.transform.position, this.transform.rotation);
        piviotTop.SetActive(false);
        piviotBottom.SetActive(false);
        foreach(Component component in deActivateList)
        {
            DisableComponent(component);
        }
        StartCoroutine(restart());
    }
    void DisableComponent(Component component)
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
    IEnumerator restart()
    {
        yield return new WaitForSeconds(3);
        uiManager.resetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
