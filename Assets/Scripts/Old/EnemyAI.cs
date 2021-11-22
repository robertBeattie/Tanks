using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 10f;
    private float rotSpeed = 60f;
    public int bulletLimit = 5;
    public int mineLimits = 2;
    public float fireRate = 10;
    private float fireRateCooldown = 0;

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
    public GameObject minePrefab;
    public Transform mineSpawnLocation;
    private GameObject player;
    public GameObject piviotTop;
    public GameObject piviotBottom;
    public bool right = false;

    private float count = 2f;
    private System.Random random = new System.Random();
    private float randomFlip;
    public LayerMask layerMask;
    bool found = false;
    WanderAI wander;
    public GameObject tracksPreFab;
    GameObject lastTrack;

    // Start is called before the first frame update
    void Start()
    {
        //Starts random aiming movment range
        //randomFlip = random.Next(0,6);
        player = GameObject.FindGameObjectWithTag("Player");
        wander = gameObject.GetComponent<WanderAI>();

        InitialTrack();

        //count timer
        count = 2f;
        right = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Aiming When to fire
        seePlayerFromAngle();
        //Cooldown
        fireRateCooldown = fireRateCooldown - Time.deltaTime;
        //tracks
        LayTracks();
    }

    private void Aim()
    {

    }
    private void Movment()
    {

    }
    void passiveAiming()
    {
        if(count > randomFlip)
        {
            right = !right;
            count = 0;
            randomFlip = 1;
        }
        count += Time.deltaTime;
        if (right)
        {
            piviotTop.transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
        }
        else
        {
            piviotTop.transform.Rotate(new Vector3(0, rotSpeed * -Time.deltaTime, 0));
        }
    }
    void SemiActiveAiming()
    {
        if (count > randomFlip)
        {
            right = !right;
            count = 0;
            randomFlip = random.Next(0, 2);
        }
        count += Time.deltaTime;
        if (right)
        {
            piviotTop.transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
        }
        else
        {
            piviotTop.transform.Rotate(new Vector3(0, rotSpeed * -Time.deltaTime, 0));
        }
    }
    void offensiveAiming()
    {
        Vector3 targetPostion = new Vector3(player.transform.position.x, piviotTop.transform.position.y, player.transform.position.z);
        piviotTop.transform.LookAt(targetPostion);
    }
    void seePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject == player)
            {
                fireBullet();
            }
        }
       
    }
    void seePlayerFromAngle()
    {
        reflexRay(bulletSpawnLocation.position, piviotTop.transform.TransformDirection(Vector3.forward), 3);
        //reflexSphereRay(bulletSpawnLocation.position, piviotTop.transform.TransformDirection(Vector3.forward), 4);
        if (!found)
        {
            passiveAiming();
            //offensiveAiming();
        }
        else
        {
            wander.enabled = false;
            StartCoroutine(aim(.5f));
        }              
    }
    //first Ray
    void reflexRay(Vector3 hitIn, Vector3 angle, int numReflections)
    {
        //reflection
        RaycastHit hitOut;
        numReflections--;
        if (Physics.Raycast(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward), out hitOut, Mathf.Infinity, layerMask))
        {
            if (hitOut.transform.gameObject.tag != "Enemy")
            {
                if (hitOut.transform.gameObject == player)
                {
                    Debug.DrawRay(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward) * hitOut.distance, Color.white);
                    found = true;
                    fireBullet();
                }
                else
                {
                    Debug.DrawRay(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward) * hitOut.distance, Color.red);
                    if (numReflections > 0)
                    {
                        reflexRay(hitOut, angle, numReflections);
                    }
                }
            }
            else
            {
                Debug.DrawRay(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward) * hitOut.distance, Color.green);
            }
        }
    }
    //Recursive Rayss
    void reflexRay(RaycastHit hitIn, Vector3 angle, int numReflections) 
    {
        //reflection
        numReflections--;
        RaycastHit hitOut;
        Vector3 angleOut = Vector3.Reflect(angle, hitIn.normal);
        if (Physics.Raycast(hitIn.point, angleOut, out hitOut, Mathf.Infinity, layerMask))
        {
            if (hitOut.transform.gameObject.tag != "Enemy")
            {
                if (hitOut.transform.gameObject == player)
                {
                    Debug.DrawRay(hitIn.point, angleOut * hitOut.distance, Color.white);
                    found = true;
                    fireBullet();
                }
                else
                {
                    Debug.DrawRay(hitIn.point, angleOut * hitOut.distance, Color.blue);
                    if (numReflections > 0)
                    {
                        reflexRay(hitOut, angleOut, numReflections);
                    }
                }  
            }
            else
            {
                Debug.DrawRay(hitIn.point, angleOut * hitOut.distance, Color.green);
            }
        }
    }
    //first Sphere Ray [WIP]
    void reflexSphereRay(Vector3 hitIn, Vector3 angle, int numReflections)
    {
        //reflection
        RaycastHit hitOut;
        numReflections--;
        if (Physics.SphereCast(piviotTop.transform.position, 0.25f, piviotTop.transform.TransformDirection(Vector3.forward), out hitOut, Mathf.Infinity, layerMask))
        {
            if (hitOut.transform.gameObject.tag != "Enemy")
            {
                if (hitOut.transform.gameObject == player)
                {
                    Debug.DrawRay(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward) * hitOut.distance, Color.white);
                    found = true;
                    fireBullet();
                }
                else
                {
                    Debug.DrawRay(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward) * hitOut.distance, Color.red);
                    if (numReflections > 0)
                    {
                        reflexSphereRay(hitOut, angle, numReflections);
                    }
                }
            }
            else
            {
                Debug.DrawRay(piviotTop.transform.position, piviotTop.transform.TransformDirection(Vector3.forward) * hitOut.distance, Color.green);
            }
        }
    }
    //Recursive Sphere Rays [WIP]
    void reflexSphereRay(RaycastHit hitIn, Vector3 angle, int numReflections)
    {
        //reflection
        numReflections--;
        RaycastHit hitOut;
        Vector3 angleOut = Vector3.Reflect(angle, hitIn.normal);
        if (Physics.SphereCast(hitIn.point, 0.25f, angleOut, out hitOut, Mathf.Infinity, layerMask))
        {
            if (hitOut.transform.gameObject.tag != "Enemy")
            {
                if (hitOut.transform.gameObject == player)
                {
                    Debug.DrawRay(hitIn.point, angleOut * hitOut.distance, Color.white);
                    found = true;
                    fireBullet();
                }
                else
                {
                    Debug.DrawRay(hitIn.point, angleOut * hitOut.distance, Color.blue);
                    if (numReflections > 0)
                    {
                        reflexSphereRay(hitOut, angleOut, numReflections);
                    }
                }
            }
            else
            {
                Debug.DrawRay(hitIn.point, angleOut * hitOut.distance, Color.green);
            }
        }
    }


    void fireBullet()
    {       
        if (fireRateCooldown <= 0)
        {
            Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
            fireRateCooldown = 3;
            //StartCoroutine(multishot(2));            
        }
    }

    IEnumerator multishot(int numShot)
    {
        for(int i = 0; i < numShot; i++)
        {
            yield return new WaitForSeconds(.1f);
            Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);

        }
    }

    IEnumerator aim(float time)
    {
        yield return new WaitForSeconds(time);
        found = false;
    }
    void InitialTrack()
    {
        lastTrack = Instantiate(tracksPreFab, piviotBottom.transform.position, piviotBottom.transform.rotation);
        lastTrack.transform.Rotate(-90, 0, 90);
        lastTrack.transform.Translate(new Vector3(0, 0, 0.1f));
    }

    void LayTracks()
    {
        if (Vector3.Distance(lastTrack.transform.position, piviotBottom.transform.position) >= .3)
        {
            GameObject track = Instantiate(tracksPreFab, piviotBottom.transform.position, piviotBottom.transform.rotation);
            track.transform.Rotate(-90, 0, 0);
            track.transform.Translate(new Vector3(0, 0, 0.1f));
            lastTrack = track;
        }
    }
}