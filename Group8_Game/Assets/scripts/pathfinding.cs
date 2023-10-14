using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinding : MonoBehaviour
{
    public Transform enemy;
    public Transform player;
    public List<Node> currentPath;

    bool followPath;
    grid grid;
    void Awake()
    {
        grid = GetComponent<grid>();
        followPath = true;
    }

    private void Update()
    {
        //Gets the current path start to finish
        currentPath = path(enemy.position, player.position);

        //If path isn't empty, then move the enemy by one node
        if (currentPath != null)
        {
            
            //Rotate enemy towards node
            Vector3 nodePosition = currentPath[0].worldPosition;
            float movementMagnitude = nodePosition.magnitude;
            nodePosition.Normalize();

            Quaternion rotateEnemy = Quaternion.LookRotation(Vector3.forward, nodePosition);


            enemy.rotation = Quaternion.RotateTowards(enemy.rotation, rotateEnemy, 5f);
            
            //Move enemy towards node
            enemy.position = Vector3.MoveTowards(enemy.position, nodePosition, 5f * Time.deltaTime);
            
        }

    }

    //Function changed to retun path to use in update
    private List<Node> path(Vector3 start, Vector3 end)
    {
        Node startNode = grid.nodeWorldPoint(start);
        Node endNode = grid.nodeWorldPoint(end);

        List<Node> nodes = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        nodes.Add(startNode);

        while (nodes.Count > 0)
        {

            Node currentNode = nodes[0];
            for (int i = 1; i < nodes.Count; i++)
            {
                if (nodes[i].fCost() < currentNode.fCost() || nodes[i].fCost() == currentNode.fCost() && nodes[i].hCost < currentNode.hCost)
                {
                    currentNode = nodes[i];
                }
            }
            nodes.Remove(currentNode);
            visited.Add(currentNode);
            if (currentNode == endNode)
            {
                retrace(startNode, endNode);
                return nodes;
            }
            foreach (Node neighbors in grid.getNeighbors(currentNode))
            {
                if (!neighbors.walkable || visited.Contains(neighbors))
                {
                    continue;
                }
                int costToNeighbor = currentNode.gCost + distance(currentNode, neighbors);
                if (costToNeighbor < neighbors.gCost || !nodes.Contains(neighbors)) 
                {
                    neighbors.gCost = costToNeighbor;
                    neighbors.hCost = distance(neighbors, endNode);
                    neighbors.parent = currentNode;
                    if (!nodes.Contains(neighbors))
                    {
                        
                        nodes.Add(neighbors);
                    }
                }
            
            }
            

        }

        return null;

        
    }

    void retrace (Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;
        while (current != start)
        {
            
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        grid.path = path;
    }


    int distance (Node a , Node b)
    {
        int disx = Mathf.Abs(a.gridx - b.gridx);
        int disy = Mathf.Abs(a .gridy - b.gridy);
        if (disx > disy)
        {
            return (14*disy + 10*(disx-disy));
        }
        return (14*disx + 10*(disy - disx));
    }

    //Changes what enemy path's target is
    public void changePathing(bool canSeePlayer)
    {
        //If able to see player, don't follow path. Else, follow path
        followPath = !canSeePlayer;
    }


}
