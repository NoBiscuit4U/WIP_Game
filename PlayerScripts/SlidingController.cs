using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingController:MonoBehaviour{
    private PlayerController playerController;
    
    private Rigidbody rigidbody;

    [Header("Sliding Vars")]
    public float slideTime;
    public float slideForce;
    public float diminishingSlidingForce;
    public float diminishingSlidingSpeed;
    public float slideTimer;
    public bool isSliding;

    private void Start(){
        playerController=GetComponent<PlayerController>();
        diminishingSlidingForce=slideForce;
        diminishingSlidingSpeed=playerController.slideSpeed;

        rigidbody=playerController.GetComponent<Rigidbody>();
    }

    private void Update(){
    }

    public void InitiateSlide(){
        isSliding=true;
        slideTimer=slideTime;

        rigidbody.AddForce(Vector3.down*20,ForceMode.Impulse);
    }

    public void StopSlide(){
        transform.localScale=new Vector3(transform.localScale.x,playerController.startYScale,transform.localScale.z);
        diminishingSlidingForce=slideForce;
        diminishingSlidingSpeed=playerController.slideSpeed;

        isSliding=false;
    }

    public void SlideForceMovement(){
        if(!playerController.IsOnSlope()){
            slideTimer-=Time.deltaTime;
            diminishingSlidingForce-=Time.deltaTime*10;
            diminishingSlidingSpeed-=Time.deltaTime*50;

            rigidbody.AddForce(playerController.moveDir.normalized*diminishingSlidingForce,ForceMode.Force);

        }else{
            rigidbody.AddForce(playerController.moveDir.normalized*slideForce,ForceMode.Force);
        }

        if(slideTimer<=0){
            StopSlide();
        }
    }
}
