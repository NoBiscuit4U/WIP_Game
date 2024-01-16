using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController:MonoBehaviour{
    private Rigidbody rigidbody;
    private CapsuleCollider collider;

    public CameraController cameraController;
    SlidingController slidingController;
    SanityHandler sanityHandler;

    public Transform orientation;
    public Transform weaponHoldPos;
    public Transform weaponHoldRot;

    [Header("Movement Vars")]
    public float speed;
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float airDrag;
    public float drag;

    public float lastSpeed;
    public float desiredSpeed;

    public float speedMultiplier;
    public float slopeSpeedMultiplier;

    [Header("Jump Vars")]
    public float jumpForce;
    public float jumpCooldown;
    public float numOfJumps;
    public float maxJumps;
    public float airSpeedMultiplier;
    public bool canJump;

    [Header("Crouch Vars")]
    public float crouchYScale;
    public float startYScale;
    public bool hasCrouched;

    [Header("Grounded Check")]
    public bool grounded;
    public LayerMask isGround;

    [Header("Slope Check")]
    public float maxSlope;
    public RaycastHit slopeCheck;
    public bool jumpingFromSlope=false;

    [Header("KeyBinds")]
    public KeyCode jumpKey=KeyCode.Space;
    public KeyCode runKey=KeyCode.LeftShift;
    public KeyCode crouchKey=KeyCode.C;
    public KeyCode slideKey=KeyCode.LeftControl;
    
    [Header("UserInputs")]
    public float xInput;
    public float yInput;
    public Vector3 moveDir;

    float playerHeight;

    [Header("Current State")]
    public MovementState state;
    public AirStates airState;

    [Header("Player Held Items")]
    public GameObject playerWeapon;
    public GameObject playerHeldWeapon;
    private Transform weaponTransform;

    [Header("Held Ammunition")]
    public bool doesHaveBaseAmmunition;
    public bool doesHaveBleedAmmunition;

    [Header("Amount of Ammunition Held")]
    public float baseAmmunitionHeld;
    public float bleedAmmunitionHeld;


    public enum MovementState{
        WALKING,RUNNING,CROUCHED,SLIDING,WALLRUNNING
    }

    public enum AirStates{
       AIR,GROUNDED
    }

    private void Start(){
        sanityHandler=this.transform.parent.GetChild(2).gameObject.GetComponent<SanityHandler>();
        slidingController=GetComponent<SlidingController>();

        rigidbody=GetComponent<Rigidbody>();
        rigidbody.freezeRotation=true;

        collider=GetComponent<CapsuleCollider>();
        playerHeight=collider.height;

        startYScale=transform.localScale.y;
        crouchYScale=transform.localScale.y*0.5f;

        runSpeed=walkSpeed*1.5f;
        crouchSpeed=walkSpeed*0.5f;

        playerHeldWeapon=Instantiate(playerWeapon,this.transform);
    }

    private void Update(){
        weaponTransform=playerHeldWeapon.transform;
        SetMovementState();
        HandleAirState();
        HandleMovementStates();
        GroundedChecks();
        CheckForInput();
        SpeedControl();

        weaponTransform.position=weaponHoldPos.position;
        weaponTransform.rotation=weaponHoldRot.rotation;
    }

    void FixedUpdate(){
        ApplyForceToPlayer();
        
        if(slidingController.isSliding){
        slidingController.SlideForceMovement();
        }
    }

    private void CheckForInput(){
        xInput=Input.GetAxisRaw("Horizontal");
        yInput=Input.GetAxisRaw("Vertical");

        moveDir=orientation.forward*yInput+orientation.right*xInput;

        if(Input.GetKeyDown(jumpKey)&&canJump&&numOfJumps!=0){
            numOfJumps=numOfJumps-1;
            ApplyJumpForce();

        }else if(numOfJumps==0){
            canJump=false;
            Invoke(nameof(ResetJump),jumpCooldown);
        }

        if(Input.GetKeyDown(crouchKey)){
            hasCrouched=true;

        }else if(Input.GetKeyUp(crouchKey)){
            transform.localScale=new Vector3(transform.localScale.x,startYScale,transform.localScale.z);
            hasCrouched=false;
        }

        if(Input.GetKeyDown(slideKey)&&(xInput!=0||yInput!=0)){
            slidingController.InitiateSlide();

        }else if(Input.GetKeyUp(slideKey)&&slidingController.isSliding){
            slidingController.StopSlide();
        }
    }

    private void ApplyForceToPlayer(){
        if(grounded){
            rigidbody.AddForce(moveDir.normalized*speed,ForceMode.Force);

            moveDir=orientation.forward*yInput+orientation.right*xInput;
            rigidbody.AddForce(moveDir.normalized*speed,ForceMode.Force);

        }else if(!grounded){
            rigidbody.AddForce(moveDir.normalized*speed*airSpeedMultiplier,ForceMode.Force);

            moveDir=orientation.forward*yInput+orientation.right*xInput;
            rigidbody.AddForce(moveDir.normalized*speed/3,ForceMode.Force);
        }

        if(IsOnSlope()&&!jumpingFromSlope){
            rigidbody.AddForce(GetSlopeMoveAdjustment(moveDir)*speed*1.5f,ForceMode.Force);

            if(rigidbody.velocity.y>0||rigidbody.velocity.x>0){
                rigidbody.AddForce(Vector3.down*30f,ForceMode.Force);
            }

            if(rigidbody.velocity.y>-0.1f){
                rigidbody.AddForce(Vector3.back*20f,ForceMode.Force);
            }
        }

        //causes problems going down slope
        rigidbody.useGravity=!IsOnSlope();
        rigidbody.AddForce(Physics.gravity*2f,ForceMode.Acceleration);
    }

    private void ApplyJumpForce(){
        jumpingFromSlope=true;

        rigidbody.velocity=new Vector3(rigidbody.velocity.x,0f,rigidbody.velocity.z);

        rigidbody.AddForce(transform.up*jumpForce,ForceMode.Impulse);
        rigidbody.AddForce(transform.forward*jumpForce/8*speed/6,ForceMode.Force);
    }

    private void ResetJump(){
        jumpingFromSlope=false;

        if(grounded||IsOnSlope()){
            numOfJumps=maxJumps;
        }
    }

    private void GroundedChecks(){
        grounded=Physics.Raycast(transform.position,Vector3.down,playerHeight*0.5f+0.1f,isGround);

        if(grounded||IsOnSlope()){
            rigidbody.drag=drag;
            Invoke(nameof(ResetJump),0);
            canJump=true;
        }
    }

    public bool IsOnGround(){
        if(grounded||IsOnSlope()){
            return true;

        }else{
            return false;
        }
    }

    public bool IsOnSlope(){
        if(Physics.Raycast(transform.position,Vector3.down,out slopeCheck,playerHeight*0.5f+0.3f)){
            float currentAngle=Vector3.Angle(Vector3.up,slopeCheck.normal);
            return currentAngle<maxSlope&&currentAngle!=0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveAdjustment(Vector3 moveDirection){
        return Vector3.ProjectOnPlane(moveDirection,slopeCheck.normal).normalized;
    }

    private void SpeedControl(){
        if(IsOnSlope()&&!jumpingFromSlope){
            if(rigidbody.velocity.magnitude>speed){
                rigidbody.velocity=rigidbody.velocity.normalized*speed;
            }

        }else{
            Vector3 flatVel=new Vector3(rigidbody.velocity.x,0f,rigidbody.velocity.z);

            if(flatVel.magnitude>speed*0.5){
                Vector3 limitedVel=flatVel.normalized*speed;
                rigidbody.velocity=new Vector3(limitedVel.x,rigidbody.velocity.y,limitedVel.z);
            }
        }
    }

    private IEnumerator LerpMoveForce(){
        float time=0;
        float difference=Mathf.Abs(desiredSpeed-speed);
        float startValue=speed;

        while(time<difference){
            speed=Mathf.Lerp(startValue,desiredSpeed,time/difference);

            if(IsOnSlope()){
                float slopeAngle=Vector3.Angle(Vector3.up,slopeCheck.normal);
                float slopeSpeedIncrease=1+(slopeAngle/90f);

                time+=Time.deltaTime*speedMultiplier*slopeSpeedMultiplier*slopeSpeedIncrease;
            }

            time+=Time.deltaTime*speedMultiplier;

            yield return null;
        }

        speed=desiredSpeed;
    }

    private void SetMovementState(){
        if(grounded&&Input.GetKey(runKey)&&!slidingController.isSliding){
            state=MovementState.RUNNING;

        }else if(slidingController.isSliding){
            state=MovementState.SLIDING;

        }else if(hasCrouched){
            state=MovementState.CROUCHED;

        }else if(grounded&&!slidingController.isSliding||IsOnSlope()&&!slidingController.isSliding){
            state=MovementState.WALKING;
        }

        if(!grounded&&!IsOnSlope()){
            airState=AirStates.AIR;

        }else if(grounded&&IsOnSlope()){
            airState=AirStates.GROUNDED;
        }
    }

    private void HandleMovementStates(){
        switch(state){
            case MovementState.WALKING:
            speed=walkSpeed;
            desiredSpeed=walkSpeed;

            break;
            case MovementState.RUNNING:
            speed=runSpeed;
            desiredSpeed=runSpeed;

            break;
            case MovementState.CROUCHED:
            speed=crouchSpeed;
            desiredSpeed=crouchSpeed;
            transform.localScale=new Vector3(transform.localScale.x,crouchYScale,transform.localScale.z);

            break;
            case MovementState.SLIDING:
            speed=slidingController.diminishingSlidingSpeed;
            transform.localScale=new Vector3(transform.localScale.x,crouchYScale,transform.localScale.z);

            if(IsOnSlope()&&rigidbody.velocity.y<0.1f){
                desiredSpeed=slidingController.diminishingSlidingSpeed;
                rigidbody.AddForce(Vector3.down*40f,ForceMode.Force);
                
            }else{
                desiredSpeed=runSpeed;
            }

            break;
            case MovementState.WALLRUNNING:
            speed=wallRunSpeed;
            desiredSpeed=wallRunSpeed;

            break;
        }

        if(Mathf.Abs(desiredSpeed-lastSpeed)>4f&&speed!=0){
            StopAllCoroutines();
            StartCoroutine(LerpMoveForce());

        }else{
            speed=desiredSpeed;
        }

        lastSpeed=desiredSpeed;
    }

    private void HandleAirState(){
        switch(airState){
            case AirStates.AIR:
            rigidbody.drag=airDrag;
            rigidbody.AddForce(Vector3.down*2f,ForceMode.Force);

            break;
            case AirStates.GROUNDED:
            break;
        }
    }

    public bool CurrentMadnessState(){
        return sanityHandler.isInMadness;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer==LayerMask.NameToLayer("Madness")&&!sanityHandler.isInMadness){
            this.gameObject.layer=LayerMask.NameToLayer("Player");
        }else{
            this.gameObject.layer=LayerMask.NameToLayer("PlayerInMadness");
        }
    }
}
