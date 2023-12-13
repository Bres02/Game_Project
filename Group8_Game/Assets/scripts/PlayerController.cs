using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] AudioSource dashSound;
    [SerializeField] AudioClip dash;
    public static StamManager sm;
    Animator anim;
    Rigidbody2D rb;
    [SerializeField] SpriteRenderer sprite;
    private Vector2 movement;
    public float MoveSpeed;
    public float WalkSpeed = 5f;
    public float RunSpeed = 8f;
    public float Accelerate, Decelerate, Cost, Regen = 0f;
    public float SpeedLimiter = 0.7f;
    float inputHorizontal;
    float inputVertical;
    public bool hidden;
    public bool lockMove;
    public bool hide_test;


    void Start()
    {
        dashSound = GetComponent<AudioSource>();
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<StamManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        MoveSpeed = WalkSpeed;
    }


    void Update()
    {
        if (!lockMove)
        {
            //Gets user inputs and places them in movement
            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");
            anim.SetFloat("X_velocity", inputHorizontal);
            anim.SetFloat("Y_velocity", inputVertical);
            if (inputHorizontal != 0)
            {
                anim.SetBool("Horizontal", (inputHorizontal != 0));
                anim.SetBool("Vertical", false);
            }
            else
            {
                anim.SetBool("Vertical", (inputVertical != 0));
                anim.SetBool("Horizontal", false);
            }
            
            //Makes new vector2 that stores the horizonal and vertical inputs and normalizes them if both are pressed to prevent diagonals being faster than normal movement
            movement = new Vector2(inputHorizontal, inputVertical).normalized;

            //Sets the move speed to either possible extremes
            if (MoveSpeed <= WalkSpeed)
            {
                MoveSpeed = WalkSpeed;

            }
            else if (MoveSpeed >= RunSpeed)
            {
                MoveSpeed = RunSpeed;

            }


            //Calls the sprint method
            Sprint();
        }

        if (hidden == true)
        {
            sprite.enabled = false;
        }
        else if (hidden == false)
        {
            sprite.enabled = true;
        }

        //Testing hide mechanic is working for enemy sight
        if (hide_test == true)
        {
            hidden = true;
        }
    }

    void FixedUpdate()
    {
        //adds player velocity as a vector2 every fixed interval
        rb.velocity = new Vector2(movement.x * MoveSpeed, movement.y * MoveSpeed);
    }

    void Sprint()
    {
        //Ramps up player movement for sprinting if able to run when they are moving
        if (sm.canRun)
        {
            if (Input.GetKey(KeyCode.Space) && MoveSpeed <= RunSpeed && (inputHorizontal !=0 || inputVertical !=0))
            {
                sm.consumeStamina(Cost);
                MoveSpeed += Accelerate * Time.deltaTime;
                if(MoveSpeed <= (WalkSpeed + .2f))
                {
                    dashSound.clip = dash;
                    dashSound.Play();
                }
            }
            else if (MoveSpeed > WalkSpeed)
            {
                MoveSpeed -= Decelerate * Time.deltaTime;
            }
        }
        else if (!sm.canRun)
        {
            if (MoveSpeed > WalkSpeed)
            {
                MoveSpeed -= Decelerate * Time.deltaTime;
            }
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Locker")
        {
            hidden = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Locker")
        {
            hidden = false;
        }
    }*/

}
