using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class grid : MonoBehaviour
{

    public LayerMask walls;
    public Vector2 worldSize;
    public float nodeSize;
    Node[,] gridArray;



    float nodeDiamiter;
    int gridsizeX, gridsizeY;

    private void Start()
    {
        
        nodeDiamiter = nodeSize * 2;
        gridsizeX = Mathf.RoundToInt(worldSize.x / nodeDiamiter);
        gridsizeY = Mathf.RoundToInt(worldSize.y / nodeDiamiter);
        createGrid();
    
    }

    private void createGrid()
    {
        gridArray = new Node[gridsizeX, gridsizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.up * worldSize.y/2;

        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiamiter + nodeSize) + Vector3.up* (y * nodeDiamiter + nodeSize);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeSize, walls));
                gridArray[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node nodeWorldPoint(Vector2 worldPoistion)
    {
        float percentx = (worldPoistion.x + worldSize.x / 2) / worldSize.x;
        float percenty = (worldPoistion.y + worldSize.y / 2) / worldSize.y;
        percentx = Mathf.Clamp01(percentx);
        percenty = Mathf.Clamp01(percenty);

        int x = (int)(Mathf.RoundToInt(gridsizeX) * percentx);
        int y = (int)(Mathf.RoundToInt(gridsizeY) * percenty);

        return gridArray[x, y];
    }

    public List<Node> getNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x==0 && y == 0)
                {
                    continue;
                }
                
                int checkx = node.gridx + x;
                int checky = node.gridy + y;
                if (checkx >=0 && checkx < gridsizeX && checky >= 0 && checky < gridsizeY)
                {
                    neighbours.Add(gridArray[checkx,checky]);
                }
            }
        }
        return neighbours;
    }

    public Transform player;
    public Transform enemy;
    public List<Node> path;

    private void OnDrawGizmos()
    {  
        Gizmos.DrawWireCube(transform.position,new Vector3(worldSize.x,worldSize.y, 0));
        if(gridArray != null)
        {
            Node playernode = nodeWorldPoint(player.position);
            Node enemynode = nodeWorldPoint(enemy.position);
            foreach (Node n in gridArray)
            {
                Gizmos.color = (n.walkable)? Color.blue:Color.red;
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
                /*if(enemynode == n)
                {
                    Gizmos.color = Color.yellow;
                }
                if (playernode == n)
                {
                    Gizmos.color = Color.green;
                }*/
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiamiter - .1f));
            }
        }
    }

}
