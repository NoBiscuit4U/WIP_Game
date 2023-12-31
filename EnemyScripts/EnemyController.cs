using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController:MonoBehaviour{
    private EnemyStats enemyStats;

    [Header("Enemy Status State")]
    public EnemyStatusStates currentStatusState;

    public enum EnemyStatusStates{
        ALIVE,DEAD
    }

    [Header("Enemy Effects")]
    public GameObject onDeathEffect;

    private void Start(){
        enemyStats=GetComponent<EnemyStats>();
    }

    private void Update(){
        EnemyStatusStateHandler();
        BaseEnemyStateMachine();
    }

    private void HandleDeath(){
        Instantiate(onDeathEffect,this.transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void EnemyStatusStateHandler(){
        if(enemyStats.HP>0){
            currentStatusState=EnemyStatusStates.ALIVE;
        }else{
            currentStatusState=EnemyStatusStates.DEAD;
        }

    }

    private void BaseEnemyStateMachine(){
        switch(currentStatusState){
            case EnemyStatusStates.ALIVE:
            break;
            case EnemyStatusStates.DEAD:
            HandleDeath();
            break;
        }

    }
}
