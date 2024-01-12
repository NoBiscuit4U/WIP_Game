using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityHandler:MonoBehaviour{
    GameObject cameraController;
    Camera camera;

    public bool isInMadness;

    public int maxSanity;
    public int currentSanity;

    private void Start(){
        cameraController=this.transform.parent.GetChild(1).gameObject;
        camera=cameraController.transform.GetChild(0).GetComponent<Camera>();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            currentSanity+=30;

        }
        currentSanity= Mathf.Clamp(currentSanity,0,maxSanity);

        HandleSanity();
    }

    private void HandleSanity(){
        if(currentSanity>=30){
            isInMadness=true;
            camera.cullingMask=LayerMask.GetMask("Default","TransparentFX","IgnoreRaycast","Ground","Water","UI","Enemy","Projectile","Madness");
        }else{
            isInMadness=false;
            camera.cullingMask=LayerMask.GetMask("Default","TransparentFX","IgnoreRaycast","Ground","Water","UI","Enemy","Projectile");
        }
    }

}
