using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemytracker : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemylocations;

    public void sreamRadious(GameObject screamer)
    {
        foreach (GameObject enemy in enemylocations) 
        {
            if (enemy.name != screamer.name && Vector2.Distance(enemy.transform.position, screamer.transform.position) < 30f)
            {
                enemy.GetComponent<pathfinding>().state = pathfinding.enemyState.search;
            }
        }
    }
}
