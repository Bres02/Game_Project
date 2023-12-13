using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemytracker : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemylocations;

    void sreamRadious(GameObject screamer)
    {
        foreach (GameObject enemy in enemylocations) 
        {
            if (enemy.transform.position != screamer.transform.position && Vector2.Distance(enemy.transform.position, screamer.transform.position) < 5f)
            {
                enemy.GetComponent<pathfinding>().state = pathfinding.enemyState.search;
            }
        }
    }
}
