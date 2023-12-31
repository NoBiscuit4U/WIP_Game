using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats:MonoBehaviour{
    [Header("Enemy Stats")]
    public float maxHP;
    public float HP;
    public float DP;
    public float speed;

    private void Start(){
        maxHP=HP;
    }

    private void Update(){
        HP=Mathf.Clamp(HP,0,maxHP);
    }
}