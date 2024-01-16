using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController:MonoBehaviour{
    EnemyStats enemyStats;
    EnemyPathFinder enemyPathFinder;

    [Header("Enemy Aggro")]
    public EnemyAggroStates currentAggroState;

    public enum EnemyAggroStates{
        DORMANT,WANDERING,SEEKING
    }

    [Header("Enemy Status")]
    public EnemyStatusStates currentStatusState;

    public enum EnemyStatusStates{
        ALIVE,DEAD
    }

    [Header("Enemy Effects")]
    public GameObject onDeathEffect;

    private void Start(){
        enemyStats=GetComponent<EnemyStats>();
        enemyPathFinder=GetComponent<EnemyPathFinder>();

    }

    private void Update(){
        EnemyStatusStateHandler();
        EnemyStatusStateMachine();
        EnemyAggroStateMachine();
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

    private void EnemyStatusStateMachine(){
        switch(currentStatusState){
            case EnemyStatusStates.ALIVE:
            break;
            case EnemyStatusStates.DEAD:
            HandleDeath();
            break;
        }
    }

    private void EnemyAggroStateMachine(){
        switch(currentAggroState){
            case EnemyAggroStates.DORMANT:
            break;
            case EnemyAggroStates.WANDERING:
            break;
            case EnemyAggroStates.SEEKING:
            break;
        }
    }
}
