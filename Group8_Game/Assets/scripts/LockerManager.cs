using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] PlayerController playerStatus;
    [SerializeField] GameObject locker;
    BoxCollider2D solidLock;
    public Vector3 lastposition;
    public bool inRange;

    void Start()
    {
        solidLock = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player in Range: Saving Position");
        lastposition = collision.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Checks if there is a player in the triggerable area
        if (collision.tag == "Player")
        {
            inRange = true;

            //If player hits E while in range, player will enter/exit locker
            if (Input.GetKeyUp(KeyCode.E) && playerStatus.hidden != true && playerStatus.lockMove != true)
            {
                solidLock.enabled = false;
                Debug.Log("E pressed: Player Entering Locker");
                player.transform.position = locker.transform.position;
                playerStatus.lockMove = true;
                playerStatus.hidden = true;
                
            } else if (Input.GetKeyUp(KeyCode.E) && playerStatus.hidden == true)
            {
                solidLock.enabled = true;
                Debug.Log("E pressed: Player Exiting Locker");
                player.transform.position = lastposition;
                playerStatus.lockMove = false;
                playerStatus.hidden = false;
                
            }
            //Prevents players from moving out of lockers due to physics upon entering them
            if (playerStatus.hidden != false && player.transform.position != locker.transform.position)
            {
                player.transform.position = locker.transform.position;
            }
        }
    }
}
