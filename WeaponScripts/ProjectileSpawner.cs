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
                ManageProjSpread(RWScript);
                Instantiate(newProjObj,spawnPos[x].position,spawnPos[x].rotation);
                currentOrientation.Rotate(0,0,0,Space.Self);
            }
        }
    }

    public GameObject CreateProj(RangedWeaponScript RWScript, GameObject projObj){
        ProjectileScript projScript=projObj.GetComponent<ProjectileScript>();

        projScript.projDamage=RWScript.projDamage;
        projScript.bleedDebuff=RWScript.bleedDebuff;
        projScript.launchOrientation=currentOrientation;
        projScript.projVelocity=RWScript.projVelocity;
        projScript.isInMadness=RWScript.IsInMadness();
        return projObj;
    }

    public void ManageProjSpread(RangedWeaponScript RWScript){
        float xPos=Random.Range(-12f+RWScript.accuracy,12f-RWScript.accuracy);
        float yPos=Random.Range(-12f+RWScript.accuracy,12f-RWScript.accuracy);
        float zPos=Random.Range(-12f+RWScript.accuracy,12f-RWScript.accuracy);

        currentOrientation.Rotate(currentOrientation.rotation.x*xPos,currentOrientation.rotation.y*yPos,currentOrientation.rotation.z*zPos,Space.Self);
    }
}
