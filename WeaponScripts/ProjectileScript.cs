using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript:MonoBehaviour{
    private Rigidbody rigidbody;

    public Transform launchOrientation;

    public GameObject onHitEffect;

    public Material enemyHitMaterial;

    [Header("Ammunition Stats")]
    public float projDamage;
    public float projVelocity;
    public bool bleedDebuff;

    private void Start(){
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
            //Debug.Log("Current Material"+enemyHitMaterial);
            enemyStats.HP-=projDamage;

        }else{
            MeshRenderer meshRenderer=collision.gameObject.GetComponent<MeshRenderer>();
            Material material=meshRenderer.materials[0];

            onHitEffect.GetComponent<ParticleSystem>().GetComponent<Renderer>().material=material;
            //Debug.Log("Current Material"+material);
        }

        Instantiate(onHitEffect,pointOfContact,rotation);
        Destroy(this.gameObject);
    }

    
}
