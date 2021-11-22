using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject piviotTop;
    public GameObject piviotBottom;
    private CharacterController characterController;
    public float speed = 5f;
    float startSpeed;
    public int bulletLimit = 5;
    public int mineLimits = 2;

    int bulletUsed = 0;
    float bulletCoolDown = 2.5f;
    float bulletTimer = 0;
    int mineUsed = 0;

    public float shootStudderTime = .3f;

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;

    public GameObject minePrefab;
    public Transform mineSpawnLocation;

    public GameObject tracksPreFab;
    private GameObject lastTrack;

    private bool pause = false;

    public LayerMask layer;

    Joystick joystick;
    [SerializeField]
    bool isJoystick = true;
    bool shotFired = false;
    // Start is called before the first frame update
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        lastTrack = Instantiate(tracksPreFab, piviotBottom.transform.position, piviotBottom.transform.rotation);
        lastTrack.transform.Rotate(-90, 0, 90);
        lastTrack.transform.Translate(new Vector3(0, 0, 0.1f));
        startSpeed = speed;
        joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause) { return; }
        //wasd move character in direction and rotate base-----------------------------------------------------------------------------------
        float x;
        float z;
        Vector3 moveDirection;
        if (!isJoystick)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
            moveDirection = new Vector3(x, 0, z);
        }
        else
        {
            x = joystick.Horizontal;
            z = joystick.Vertical;
            moveDirection = new Vector3(x, 0, z);
        }

        if (Mathf.Abs(x) > 0 && Mathf.Abs(z) > 0)
        {
            moveDirection /= Mathf.Sqrt(2);
        }
        characterController.Move(moveDirection * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        //Lay down tacks
        if (Vector3.Distance(lastTrack.transform.position, piviotBottom.transform.position) >= .3)
        {
            GameObject track = Instantiate(tracksPreFab, piviotBottom.transform.position, piviotBottom.transform.rotation);
            track.transform.Rotate(-90, 0, 0);
            track.transform.Translate(new Vector3(0, 0, 0.1f));
            lastTrack = track;
        }
        //if there is new input move
        if (moveDirection != Vector3.zero)
        {
            Quaternion piviotBottomToRoate = Quaternion.LookRotation(moveDirection);
            piviotBottom.transform.rotation = Quaternion.Slerp(piviotBottom.transform.rotation, piviotBottomToRoate, speed * Time.deltaTime);
        }
        //top look at mouse-------------------------------------------------------------------------------------------------------------------
        RaycastHit hit;
        Ray ray;
        if (!isJoystick)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float height = piviotTop.transform.position.y;
            //LayerMask layer = LayerMask.GetMask
            if (Physics.Raycast(ray, out hit, 100f, layer) && Time.timeScale != 0)
            {
                piviotTop.transform.LookAt(new Vector3(hit.point.x, height, hit.point.z));
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console
                //Debug.Log(hit.point);
            }
            else
            {
                piviotTop.transform.LookAt(new Vector3(piviotTop.transform.position.x, height, piviotTop.transform.position.z));
            }
        }
        else
        {
            if (Input.touchCount > 1 && !shotFired)
            {
                Touch touch = Input.GetTouch(1);
                ray = Camera.main.ScreenPointToRay(touch.position);
                float height = piviotTop.transform.position.y;
                //LayerMask layer = LayerMask.GetMask
                if (Physics.Raycast(ray, out hit, 100f, layer) && Time.timeScale != 0)
                {
                    piviotTop.transform.LookAt(new Vector3(hit.point.x, height, hit.point.z));
                    Debug.DrawLine(ray.origin, hit.point);
                    //log hit area to the console
                    //Debug.Log(hit.point);
                    if (touch.phase == TouchPhase.Began)
                    {
                        FireBullet();
                    }

                }
                else
                {
                    piviotTop.transform.LookAt(new Vector3(piviotTop.transform.position.x, height, piviotTop.transform.position.z));
                }

            }
        }
        /*
        float height = piviotTop.transform.position.y;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 camRot = Camera.main.transform.rotation.eulerAngles;
        float t = -mousePos.z / camRot.z;
        Vector3 target = mousePos + (t * camRot);
        //Vector3 point = new Vector3(0, Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg, 0);
        piviotTop.transform.LookAt(new Vector3(target.x, height, target.z));
        */
        //check mouse0 to shoot------------------------------------------------------------------------------------------------------------
        if (Input.GetButtonDown("Fire1") && !isJoystick)
        {
            Debug.Log("fire bullet");
            FireBullet();
        }
        //check mouse1 to lay mine--------------------------------------------------------------------------------------------------------------------
        if (Input.GetButtonDown("Fire2") && !isJoystick)
        {
            Debug.Log("Lay Mine");
            LayMine();
        }
        if (bulletTimer >= bulletCoolDown)
        {
            bulletUsed = 0;
            bulletTimer = 0;
        }
        bulletTimer += Time.deltaTime;
        
    }
    IEnumerator studder(float shootStudderTime)
    {
        speed = 0;
        characterController.enabled = false;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);

        yield return new WaitForSeconds(.3f);
        characterController.enabled = true;
        speed = startSpeed;
    }
    
    public void Pause(bool pause)
    {
        this.pause = pause;

        if (pause)
        {
            //speed = 0;
        }
        else
        {
            //speed = startSpeed;
        }
        if (characterController != null)
        {
            characterController.enabled = !pause;
        }
    }
    public void FireBullet()
    {
        if (bulletUsed < bulletLimit && Time.timeScale != 0)
        {
            StartCoroutine(studder(shootStudderTime));
            bulletTimer = 0;
            bulletUsed++;
        }
    }
    public void LayMine()
    {
        if (mineUsed < mineLimits && Time.timeScale != 0)
        {
            GameObject mine = Instantiate(minePrefab, mineSpawnLocation.position, new Quaternion());
            mineUsed++;
        }
    }
}


