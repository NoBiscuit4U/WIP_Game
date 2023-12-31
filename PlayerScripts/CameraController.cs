using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController:MonoBehaviour{
    public Transform orientation;
    public Transform playerCameraPos;
    public Transform cameraPos;
    
    public float xSens;
    public float ySens;

    float camXRot;
    float camYRot;

    private void Start(){
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible=false;
    }

    private void Update(){
        float mouseX=Input.GetAxisRaw("Mouse X")*Time.deltaTime*xSens;
        float mouseY=Input.GetAxisRaw("Mouse Y")*Time.deltaTime*ySens;

        camYRot+=mouseX;
        camXRot-=mouseY;

        camXRot= Mathf.Clamp(camXRot,-90f,90f);
        
        transform.rotation=Quaternion.Euler(camXRot,camYRot,0);
        orientation.rotation=Quaternion.Euler(0,camYRot,0);

        playerCameraPos.position=cameraPos.position;
    }
}
