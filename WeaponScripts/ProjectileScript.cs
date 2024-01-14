using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript:MonoBehaviour{
    private Rigidbody rigidbody;

    public Transform launchOrientation;
    public Transform newPos;

    public GameObject onHitEffect;

    public Material enemyHitMaterial;

    public bool isInMadness;
    public bool canCollide;
    public bool objectCheck;

    [Header("Ammunition Stats")]
    public float projDamage;
    public float projVelocity;
    public bool bleedDebuff;

    void Start(){
        rigidbody=GetComponent<Rigidbody>();

        rigidbody.AddForce(launchOrientation.forward*projVelocity,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision){
        ContactPoint contact=collision.contacts[0];
        Quaternion rotation=Quaternion.FromToRotation(Vector3.up,contact.normal);
        Vector3 pointOfContact=contact.point;

        if(collision.gameObject.layer==LayerMask.NameToLayer("Enemy")){
            EnemyStats enemyStats=collision.gameObject.GetComponent<EnemyStats>();
            onHitEffect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material=enemyHitMaterial;
            enemyStats.HP-=projDamage;

            Instantiate(onHitEffect,pointOfContact,rotation);
            Destroy(this.gameObject);

        }else if(collision.gameObject.layer==LayerMask.NameToLayer("Madness")){
            if(canCollide){
                MeshRenderer meshRenderer=collision.gameObject.GetComponent<MeshRenderer>();
                Material material=meshRenderer.materials[0];

                onHitEffect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material=material;

                Instantiate(onHitEffect,pointOfContact,rotation);
                Destroy(this.gameObject);
            }

        }else{
            MeshRenderer meshRenderer=collision.gameObject.GetComponent<MeshRenderer>();
            Material material=meshRenderer.materials[0];

            onHitEffect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material=material;

            Instantiate(onHitEffect,pointOfContact,rotation);
            Destroy(this.gameObject);
    
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer==LayerMask.NameToLayer("Madness")){
            Debug.Log("Detected Other Collider");
            objectCheck=true;
        }else{
            objectCheck=false;
        }

        HandleMadnessCollision();
    }

    private void HandleMadnessCollision(){
        if(objectCheck==true&&!isInMadness){
            Debug.Log("Passing Through Object");
            rigidbody.detectCollisions=false;
            canCollide=false;

        }else if(objectCheck==true&&isInMadness){
            Debug.Log("Can Collide"+objectCheck+isInMadness);
            canCollide=true;
            rigidbody.detectCollisions=true;

        }else{
            canCollide=true;
            rigidbody.detectCollisions=true;

        }
    }
}
