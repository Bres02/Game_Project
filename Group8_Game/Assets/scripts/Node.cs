using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridx;
    public int gridy;

    public int gCost;
    public int hCost;

    public Node parent;

    public Node(bool walkable, Vector3 worldPosition, int gridx, int gridy)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridx = gridx;
        this.gridy = gridy;
    }

    public int fCost()
    {
        return hCost + gCost;
    }
}
