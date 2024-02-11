using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNodePreset:MonoBehaviour{
    private Transform[] nodeSnapPositions;

    public enum MAPNODELAYER{
        LAYER1,LAYER2,LAYER3,LAYER4
    }
    private MAPNODELAYER nodeLayer;

    public Transform[] getSnapPositions(){
        return nodeSnapPositions;
    }

    public MAPNODELAYER getNodeLayer(){
        return nodeLayer;
    }
}