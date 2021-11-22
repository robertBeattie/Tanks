using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Numerics;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10;
    public int ricochetLimit = 1;
    public int ricochetcount = 0;
    public GameObject explotionPreFab;
    private GameObject lastCollision;
    private Vector3 lastPostion;
    private bool pause = false;
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        lastPostion = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (pause) { return; }
        FindBullets();

        rb.MovePosition(transform.position + transform.right * speed * Time.deltaTime);
        if (ricochetcount > ricochetLimit)
        {
            GameObject explotion = Instantiate(explotionPreFab, this.transform.position, this.transform.rotation);
            DestroyImmediate(this.gameObject);
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (tag != "Player" ||
            tag != "Bullet" ||
            tag != "Mine"   ||
            tag != "Enemy" && lastCollision != collision.gameObject && Vector3.Distance(this.transform.position, lastPostion) > 0.01f)
        {
            lastCollision = collision.gameObject;
            lastPostion = this.transform.position;
            rb.isKinematic = true;
            Vector3 v = Vector3.Reflect(-this.transform.forward, collision.contacts[0].normal);
            float rot = 90 - Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
            ricochetcount++;
            rb.isKinematic = false;
        }    
    }
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Player" ||
            (tag == "Bullet" && other.gameObject != this.gameObject)||
            tag == "Mine" ||
            tag == "Enemy")
        {
            if (tag == "Player")
            {
                other.gameObject.GetComponent<PlayerDeath>().death();
            }
            else if (tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyDeath>().death();
            }
            Detinate();
        }
    }
    public void Detinate()
    {
        ricochetcount = 20000000;
    }
    private void FindBullets()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            if (bullet.gameObject == this.gameObject) { return; }
            float dist = Vector3.Distance(bullet.transform.position, transform.position);
            if (dist <= .5f)
            {
                Detinate();
                bullet.GetComponent<Bullet>().Detinate();
            }
        }
    }
    public void Pause(bool pause)
    {
        this.pause = pause;
    }
}
