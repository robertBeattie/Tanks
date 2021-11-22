using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    //10second timer to explode or when hit
    float count = 0;
    public GameObject explotionPreFab;
    public GameObject explotionFinderPreFab;
    public float timer = 10f;
    public Material matR;
    public Material matY;
    bool flash = false;
    float flashWait;
    bool detonated = false;

    void Start()
    {
        flashWait = timer - 2;
    }
    // Update is called once per frame
    void Update()
    {
        if(count > timer && !detonated)
        {
            detonated = true;
            GameObject explotion = Instantiate(explotionPreFab, this.transform.position, this.transform.rotation);
            GameObject explotionFinder = Instantiate(explotionFinderPreFab, this.transform.position, this.transform.rotation);
            DestroyImmediate(this.gameObject);
            return;
        }
        if (count >= flashWait)
        {
            if (flash)
            {
                GetComponent<Renderer>().material.CopyPropertiesFromMaterial(matR);
            }
            else
            {
                GetComponent<Renderer>().material.CopyPropertiesFromMaterial(matY);
            }
            flashWait += .075f;
            flash = !flash;
        }
        count += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Bullet" )
        {
           other.GetComponent<Bullet>().ricochetcount = 20;
           Detinate();
        }
    }
    public void Detinate()
    {
        count = 20;
    }
}
