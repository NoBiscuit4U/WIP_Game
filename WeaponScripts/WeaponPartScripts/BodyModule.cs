using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyModule:MonoBehaviour{
    [Header("Body Module Stats")]
    public float rateOfFire;

    [Header("Body Module Unique")]
    public bool overPressurize;

    [Header("Module Attach Points")]
    public Transform barrelAttachPoint;
    public Transform ammoAttachPoint;
    public Transform stockAttachPoint;
    public Transform sightAttachPoint;

}
