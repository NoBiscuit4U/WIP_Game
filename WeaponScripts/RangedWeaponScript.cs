using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponScript:MonoBehaviour{
    private ProjectileSpawner spawnProj;
    public PlayerController playerController;
    public CameraController cameraController;

    [Header("Weapon Modules")]
    public GameObject barrelModule;
    public GameObject bodyModule;
    public GameObject ammoModule;
    public GameObject stockModule;
    public GameObject sightModule;

    [Header("Weapon Module Snap Points")]
    public Transform barrelAttachPoint;
    public Transform ammoAttachPoint;
    public Transform stockAttachPoint;
    public Transform sightAttachPoint;

    [Header("Ammo Stats")]
    public float magSize;
    public float numOfCurrentBullets;

    [Header("Barrel Stats")]
    public Transform[] projSpawnPos;
    public float numOfBarrels;
    public float projVelocity;
    public float recoil;

    [Header("Body Stats")]
    public float rateOfFire;
    public bool overPressurize;

    [Header("Stock Stats")]
    public float weaponStability;

    [Header("Sight Stats")]
    public float accuracy;

    [Header("Ammunition Stats")]
    public float projDamage;
    public float numberOfProj;
    public bool bleedDebuff;

    [Header("Currently Used Ammo")]
    public string currentAmmo;

    private void Start(){
        spawnProj=GetComponent<ProjectileSpawner>();
        playerController=this.transform.parent.gameObject.GetComponent<PlayerController>();
        cameraController=this.transform.parent.gameObject.GetComponent<PlayerController>().cameraController;
        barrelModule=this.transform.GetChild(0).gameObject;
        bodyModule=this.transform.GetChild(1).gameObject;
        ammoModule=this.transform.GetChild(2).gameObject;
        stockModule=this.transform.GetChild(3).gameObject;
        sightModule=this.transform.GetChild(4).gameObject;

        ExtractModuleStats();
        AttachModules();

        numOfCurrentBullets=magSize;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse0)&&numOfCurrentBullets!=0){
            numOfCurrentBullets-=1;
            spawnProj.currentOrientation=projSpawnPos[0];
            spawnProj.SpawnProjectile(this,numberOfProj,numOfBarrels,projSpawnPos);
            RecoilForce();
        }

        if(Input.GetKeyDown(KeyCode.R)){
            numOfCurrentBullets=magSize;
        }
    }

    private void ExtractModuleStats(){
        BarrelModule barrelScript=barrelModule.GetComponent<BarrelModule>();
        BodyModule bodyScript=bodyModule.GetComponent<BodyModule>();
        AmmoModule ammoScript=ammoModule.GetComponent<AmmoModule>();
        StockModule stockScript=stockModule.GetComponent<StockModule>();
        SightModule sightScript=sightModule.GetComponent<SightModule>();

        projVelocity=barrelScript.projVelocity;
        numOfBarrels=barrelScript.numOfBarrels;
        projSpawnPos=barrelScript.projSpawnPos;
        recoil=barrelScript.recoil;

        rateOfFire=bodyScript.rateOfFire;
        overPressurize=bodyScript.overPressurize;
        barrelAttachPoint=bodyScript.barrelAttachPoint;
        ammoAttachPoint=bodyScript.ammoAttachPoint;
        stockAttachPoint=bodyScript.stockAttachPoint;
        sightAttachPoint=bodyScript.sightAttachPoint;

        magSize=ammoScript.magSize;

        weaponStability=stockScript.weaponStability;
        
        accuracy=sightScript.accuracy;
    }

    private void AttachModules(){
        barrelModule.transform.position=barrelAttachPoint.position;
        ammoModule.transform.position=ammoAttachPoint.position;
        stockModule.transform.position=stockAttachPoint.position;
        sightModule.transform.position=sightAttachPoint.position;
    }

    private void GetAmmuntionStats(){
        
    }

    public void RecoilForce(){
        float recoilForce=recoil-weaponStability;
        
        if(recoilForce<0){
            recoilForce=0;
        }

        cameraController.camXRot=cameraController.camXRot+-recoilForce*0.2f;
    }

    public bool IsInMadness(){
        return playerController.CurrentMadnessState();
    }
}
