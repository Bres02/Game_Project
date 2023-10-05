using System.Collections;
using System.Collections.Generic;
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
                bool walkable = !(Physics2D.OverlapCircle(worldPoint,nodeSize,walls));
                gridArray[x, y] = new Node(walkable, worldPoint);
            }
        }
    }

    private void OnDrawGizmos()
    {  
        Gizmos.DrawWireCube(transform.position,new Vector3(worldSize.x,worldSize.y,0));
        if(gridArray != null)
        {
            foreach (Node n in gridArray)
            {
                Gizmos.color = (n.walkable)? Color.blue:Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiamiter - .1f));
            }
        }
    }

}
