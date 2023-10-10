using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class grid : MonoBehaviour
{

    public LayerMask walls;
    public Vector2 worldSize;
    public float nodeRadius; 
    Node[,] gridArray;



    float nodeDiamiter;
    int gridizeX, gridizeY;

    private void Start()
    {
        
        nodeDiamiter = nodeRadius * 2;
        gridizeX = Mathf.RoundToInt(worldSize.x / nodeDiamiter);
        gridizeY = Mathf.RoundToInt(worldSize.y / nodeDiamiter);
        createGrid();
    
    }

    private void createGrid()
    {
        gridArray = new Node[gridizeX, gridizeY];
       
        //calculates the bottom left so it can generate the array
        Vector3 worldBottomLeft = transform.position - Vector3.right * worldSize.x / 2 - Vector3.up * worldSize.y/2;

        for (int x = 0; x < gridizeX; x++)
        {
            for (int y = 0; y < gridizeY; y++)
            {
                //calcualtes the loaction of a node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiamiter + nodeRadius) + Vector3.up* (y * nodeDiamiter + nodeRadius);
                
                //see node
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, walls));
                gridArray[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }


    //gets the posistion of a given node
    public Node nodeWorldPoint(Vector2 worldPoistion)
    {
        float percentx = (worldPoistion.x + worldSize.x / 2) / worldSize.x;
        float percenty = (worldPoistion.y + worldSize.y / 2) / worldSize.y;
        percentx = Mathf.Clamp01(percentx);
        percenty = Mathf.Clamp01(percenty);

        int x = (int)(Mathf.RoundToInt(gridizeX) * percentx);
        int y = (int)(Mathf.RoundToInt(gridizeY) * percenty);

        return gridArray[x, y];
    }

    //finds the neighboring nodes of a given node
    public List<Node> getNeighbors(Node node)
    {
        List<Node> neighbours = new List<Node>();
        
        //starts at -1 so since that is the bottom left spot
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //this is to skip the node that we are looking for neighbors of
                if (x==0 && y == 0)
                {
                    continue;
                }
                
                int checkx = node.gridx + x;
                int checky = node.gridy + y;
                
                //if the node is in the array list adds it to the list
                if (checkx >=0 && checkx < gridizeX && checky >= 0 && checky < gridizeY)
                {
                    neighbours.Add(gridArray[checkx,checky]);
                }
            }
        }
        return neighbours;
    }

    public List<Node> path;
    //used to display the nodes
    private void OnDrawGizmos()
    {  
        Gizmos.DrawWireCube(transform.position,new Vector3(worldSize.x,worldSize.y, 0));
        if(gridArray != null)
        {
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
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiamiter - .2f));
            }
        }
    }

}
