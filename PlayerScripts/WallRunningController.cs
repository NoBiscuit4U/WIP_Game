using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Video url https://www.youtube.com/watch?v=gNt9wBOrQO4

public class WallRunningController:MonoBehaviour{
    private PlayerController playerController;

    private Rigidbody rigidbody;

    [Header("Wallrunning Vars")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallRunTimer;
    public bool isWallRunning;

    [Header("Wallrunning Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    public RaycastHit leftWallHit;
    public RaycastHit rightWallHit;
    public bool wallLeft;
    public bool wallRight;

    private void Start(){
        playerController=GetComponent<PlayerController>();

        rigidbody=playerController.GetComponent<Rigidbody>();
    }

    private void Update(){
        CheckForWall();
    }

    private void CheckForWall(){
        wallRight=Physics.Raycast(transform.position,playerController.orientation.right,out rightWallHit,wallCheckDistance,whatIsWall);
        wallLeft=Physics.Raycast(transform.position,-playerController.orientation.right,out leftWallHit,wallCheckDistance,whatIsWall);
    }

    public bool ShouldChangeToWallRunState(){
        if((wallLeft||wallRight)&&playerController.yInput>0&&playerController.IsOnGround()){
           return true;
        }
        return false;
    }

    public void StartWallRunning(){
        isWallRunning=true;
    }

    public void StopWallRunning(){

    }

    public void WallRunningForceMovement(){

    }
}
