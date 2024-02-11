using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator:MonoBehaviour{
    [Header("Map Generation Nodes")]
    public MapNodePreset[] allNodes;
    public MapNodePreset[] availableNodesLayer1;
    public MapNodePreset[] availableNodesLayer2;
    public MapNodePreset[] availableNodesLayer3;
    public MapNodePreset[] availableNodesLayer4;

    [Header("Node Snap Locations")]
    public Transform[] allNodeSnap;

    public enum MAPNODELAYER{
        LAYER1,LAYER2,LAYER3,LAYER4
    }

    public MAPNODELAYER currentGenerationLayer;

    public void SortMapGenerationNodes(){}
}