using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    public float count = 0f;
    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count >= 1.9f)
        {
            DestroyImmediate(this.gameObject);
        }
    }
}
