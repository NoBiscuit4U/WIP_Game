using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPositon;
    public int gridX;
    public int gridY;
    public int movementPenalty;
    
    public int gCost;
    public int hCost;
    public Node parent;
    public int heapIndex;

    public Node(bool _walkable, Vector3 _worldpos, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPositon = _worldpos;
        gridY = _gridY;
        gridX = _gridX;
        movementPenalty = _penalty;
    }

    public int fCost
    {
        get{
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get{
            return heapIndex;
        }
        set{
            heapIndex = value;
        }
    }

     public int CompareTo(Node nodeToCompare)
     {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        
         return -compare;
     }
}
