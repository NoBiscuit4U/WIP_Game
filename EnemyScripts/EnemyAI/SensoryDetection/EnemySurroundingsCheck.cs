using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySurroundingsCheck:MonoBehaviour{
    public LayerMask[] walkableLayerMasks;

    float lineCastLength=5.0f;

    public bool IsWallToLeft(float enemySize){
        return Physics.Raycast(this.transform.position,Vector3.left,enemySize);
    }

    public bool IsWallToRight(float enemySize){
        return Physics.Raycast(this.transform.position,Vector3.right,enemySize);
    }

    public bool IsWallForward(float enemySize){
        return Physics.Raycast(this.transform.position,Vector3.forward,enemySize);
    }

    public bool IsWallBackward(float enemySize){
        return Physics.Raycast(this.transform.position,Vector3.back,enemySize);
    }

    public bool IsPlayerInView(Transform player){
        return Physics.Linecast(this.transform.position,player.position,LayerMask.NameToLayer("Player"));  
    }
}
