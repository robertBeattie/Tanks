using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionFinder : MonoBehaviour
{
    //crates,players,ememeys, bulllets, mines
    // Start is called before the first frame update
    //public float deathTimer = .7f;
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 25, 0), ForceMode.Acceleration);
        if (transform.position.y >= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Player" ||
            tag == "Bullet" ||
            tag == "Mine"   ||
            tag == "Enemy"  ||
            tag == "Crate")
        {
            if(tag == "Player")
            {
                other.gameObject.GetComponent<PlayerDeath>().death();
            }
            else if(tag == "Bullet")
            {
                other.gameObject.GetComponent<Bullet>().Detinate();
            }
            else if(tag == "Mine")
            {
                other.gameObject.GetComponentInParent<Mine>().Detinate();
            }
            else if(tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyDeath>().death();
            }
            else if(tag == "Crate")
            {
                other.gameObject.SetActive(false);
            }    
        }
    }
}
