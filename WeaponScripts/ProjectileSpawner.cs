using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner:MonoBehaviour{
    public GameObject blankProjObj;

    public Transform currentOrientation;

    private void Start(){
    }
    
    private void Update(){
    }

    public void SpawnProjectile(RangedWeaponScript RWScript,float numOfProj,float numOfBarrels,Transform[] spawnPos){
        numOfProj=RWScript.numberOfProj;

        GameObject newProjObj=CreateProj(RWScript,blankProjObj);

        for(int i=0;i<numOfProj;i++){
            for(int x=0;x<numOfBarrels;x++){
                spawnPos[x]=ManageProjSpread(currentOrientation);
                Instantiate(newProjObj,spawnPos[x].position,spawnPos[x].rotation);
            }
        }
    }

    public GameObject CreateProj(RangedWeaponScript RWScript, GameObject projObj){
        ProjectileScript projScript=projObj.GetComponent<ProjectileScript>();

        projScript.projDamage=RWScript.projDamage;
        projScript.bleedDebuff=RWScript.bleedDebuff;
        projScript.launchOrientation=currentOrientation;
        projScript.projVelocity=RWScript.projVelocity;
        return projObj;
    }

    public Transform ManageProjSpread(Transform pos){
        float xPos=Random.Range(9f,10f);
        float yPos=Random.Range(9f,10f);
        float zPos=Random.Range(9f,10f);

        pos.rotation.eulerAngles.Set(currentOrientation.rotation.x*xPos,currentOrientation.rotation.y*yPos,currentOrientation.rotation.z*zPos);
        return pos;
    }
}
