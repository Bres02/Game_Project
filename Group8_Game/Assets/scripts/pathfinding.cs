using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinding : MonoBehaviour
{
    public Transform enemy;
    public Transform player;


    grid grid;
    Node order;

    [SerializeField] private GameObject gameManager;

    void Awake()
    {
        grid = gameManager.GetComponent<grid>();
    }

    private void Update()
    {
        path(enemy.position, player.position);

        if (player.position != enemy.position && order != null)
        {
           transform.position = Vector2.MoveTowards(transform.position, order.worldPosition, 1.5f * Time.deltaTime);
        }
    }

    void path(Vector3 start, Vector3 end)
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
                return;
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
        order = path[0];
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
}
