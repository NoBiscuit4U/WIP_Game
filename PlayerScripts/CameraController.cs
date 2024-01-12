using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController:MonoBehaviour{
    public Transform orientation;
    public Transform playerCameraPos;
    public Transform cameraPos;
    public Transform cameraTransform;
    
    public float xSens;
    public float ySens;

    public float camXRot;
    public float camYRot;

    private void Start(){
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible=false;
    }

    private void Update(){
        float mouseX=Input.GetAxisRaw("Mouse X")*Time.deltaTime*xSens;
        float mouseY=Input.GetAxisRaw("Mouse Y")*Time.deltaTime*ySens;

        camYRot+=mouseX;
        camXRot-=mouseY;

        camXRot=Mathf.Clamp(camXRot,-90f,90f);
        
        playerCameraPos.rotation=Quaternion.Euler(camXRot,camYRot,0);
        orientation.rotation=Quaternion.Euler(0,camYRot,0);

        playerCameraPos.position=cameraPos.position;
    }
}
