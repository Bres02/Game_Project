using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    enum enemyState { patrol, chase, search };
    LevelManager levelManager;

    [SerializeField] AudioSource detectedSound;
    [SerializeField] AudioClip detected;
    [SerializeField] AudioClip scream;

    //movement values
    Vector2 target;
    public float WalkSpeed = 5f;
    //public float RunSpeed = 8f;
    public float SpeedLimiter = 0.7f;
    float inputHorizontal;
    float inputVertical;

    //Establishes values used for detection
    public float viewRadius = 5f;
    [Range(1, 360)] public float viewAngle = 45f;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject gameManeger;
    [SerializeField] public bool isWarrior;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask wallMask;
    public bool canSeePlayer;
    private int detectCounter = 0;
    private int screamCounter = 0;

    private bool didScream = false;

    [SerializeField] bool isDemo;
    // Start is called before the first frame update
    void Start()
    {
        //Sets the reference to player and starts the FOVRoutine at the start
        detectedSound = GetComponent<AudioSource>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    //Has the enemy run field of view every 0.2 seconds instead of every frame
    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }


    // Enemy Line of Sight
    private void FieldOfViewCheck()
    {
        //Creats an array for objects in the target layer that are within the viewable radius of the object
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        detectCounter = 0;
        screamCounter = 0;
        //If the array's length isn't 0, than an object in the target layer is in the radius
        if (rangeCheck.Length != 0)
        {
            //Checks the direction and angle target is in relation to object
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            //Sees if the target is within viewable area, if not it doesn't see target
            if (Vector2.Angle(transform.up, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                //Checks to see if there is an object on wall layer between it and target, if not it sees target
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallMask))
                {
                    detectCounter += 1;
                    screamCounter += 1;
                    if (player.hidden == false)
                    {
                        canSeePlayer = true;
                        if (canSeePlayer && detectCounter  < 2)
                        {
                            detectedSound.clip = detected;
                            detectedSound.Play();
                        }
                        this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.chase;
                        if (!didScream)
                        {

                            gameManeger.GetComponent<enemytracker>().sreamRadious(this.gameObject);
                            didScream = true;
                            if(screamCounter < 2)
                            {
                                detectedSound.clip = scream;
                                detectedSound.Play();
                            }
                        }

                    }
                    else
                    {
                        canSeePlayer = false;
                        if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.search)
                        {

                        }else if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.chase)
                        {
                            this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.search;
                        }
                        else
                        {
                            this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.patrol;
                        }
                    }

                }
                else
                {
                    detectedSound.Pause();
                    canSeePlayer = false;
                    if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.search)
                    {

                    }
                    else if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.chase)
                    {
                        this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.search;
                    }
                    else
                    {
                        this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.patrol;
                        if (didScream)
                        {
                            didScream = false;
                        }
                    }
                }
            }
            else
            {
                canSeePlayer = false;
                if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.search)
                {

                }
                else if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.chase)
                {
                    this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.search;
                }
                else
                {
                    this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.patrol;
                    if (didScream)
                    {
                        didScream = false;
                    }
                }
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.search)
            {

            }
            else if (this.GetComponent<pathfinding>().state == (pathfinding.enemyState)enemyState.chase)
            {
                this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.search;
            }
            else
            {
                this.GetComponent<pathfinding>().state = (pathfinding.enemyState)enemyState.patrol;
                if (didScream)
                {
                    didScream = false;
                }
            }
            Debug.Log("Enemy has lost sight of player");
        }
    }


    //Shows the objects viewable radius and the cone of sight for debug purposes
    private void OnDrawGizmos()
    {
        //Shows the radius as a white circle
        //Gizmos.color = Color.white;
        //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, viewRadius);

        //Shows the cone of view as two green lines
        Vector3 viewAngle1 = DirectionFromviewAngle(-transform.eulerAngles.z, -viewAngle * 0.5f);
        Vector3 viewAngle2 = DirectionFromviewAngle(-transform.eulerAngles.z, viewAngle * 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle1 * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle2 * viewRadius);

        //If the object can see player, it draws a red line towards player
        if (canSeePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }

    }
    //Method called above to calculate the angles of sight, place here to make format easier to read
    private Vector2 DirectionFromviewAngle(float eulerY, float viewAngleDegrees)
    {
        viewAngleDegrees += eulerY;
        return new Vector2(Mathf.Sin(viewAngleDegrees * Mathf.Deg2Rad), Mathf.Cos(viewAngleDegrees * Mathf.Deg2Rad));
    }


    // Update is called once per frame
    void Update()
    {

        if (canSeePlayer == false && GetComponent<pathfinding>().order.Count >0)
        {
            Vector3 Look = transform.InverseTransformPoint(GetComponent<pathfinding>().order[0].worldPosition);
            float targetAngle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg - 90;
            transform.Rotate(0, 0, targetAngle);
        }
        //If able to see player, object will rotate to follow the player position
        else if (canSeePlayer)
        {
            Vector3 Look = transform.InverseTransformPoint(playerRef.transform.position);
            float targetAngle = Mathf.Atan2(Look.y, Look.x) * Mathf.Rad2Deg - 90;
            transform.Rotate(0, 0, targetAngle);
        }
       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

}