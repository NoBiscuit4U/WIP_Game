using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController:MonoBehaviour{
    EnemyStats enemyStats;
    EnemyMovementHandler enemyMovement;
    EnemySurroundingsCheck enemyDetection;

    bool hasStartedWandering=false;
    bool isAttackingPlayer;

    [Header("Targets and CurrentPos")]
	public Transform[] playerTargets;
	public Transform[] wanderingTargets;
    public Transform enemyCurrentPos;
	public Transform target;

    [Header("Enemy Aggro")]
    public EnemyAggroStates currentAggroState;

    public enum EnemyAggroStates{
        DORMANT,WANDERING,SEEKING
    }

    [Header("Enemy Effects")]
    public GameObject onDeathEffect;

    private void Start(){
        enemyStats=GetComponent<EnemyStats>();
        enemyDetection=this.transform.GetChild(0).GetComponent<EnemySurroundingsCheck>();
        enemyMovement=GetComponent<EnemyMovementHandler>();
    }

    private void Update(){
        if(enemyStats.HP<=0){
            HandleDeath();
        }

        if(isAttackingPlayer&&IsInAttackRange()){

        }

        EnemyAggroStateMachine();
    }

    private void HandleDeath(){
        Instantiate(onDeathEffect,this.transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
    
    private void EnemyAggroStateMachine(){
        switch(currentAggroState){
            case EnemyAggroStates.DORMANT:
            enemyMovement.StateToIdle();
            StartWanderingRoutine(true);
            break;
            case EnemyAggroStates.WANDERING:
            enemyMovement.SetTarget(target);
            enemyMovement.StateToSeeking();
            StartWanderingRoutine(false);
            break;
            case EnemyAggroStates.SEEKING:
            HandlePlayerTargeting();
            isAttackingPlayer=true;
            enemyMovement.SetTarget(target);
            enemyMovement.StateToSeeking();
            StartWanderingRoutine(true);
            break;
        }
    }
    
    private void StartWanderingRoutine(bool stop){
        if(hasStartedWandering==false){
            hasStartedWandering=true;
            StartCoroutine(HandleEnemyWandering());
        }

        if(stop==true){
            hasStartedWandering=false;
            StopCoroutine(HandleEnemyWandering());
        }
    }

    private IEnumerator HandleEnemyWandering(){
        for(int i=0;i<wanderingTargets.Length;){
            target=wanderingTargets[i];
            yield return new WaitForSeconds(0.01f);

            if(enemyCurrentPos.position.x==wanderingTargets[i].position.x&&enemyCurrentPos.position.z==wanderingTargets[i].position.z){
                i++;
            }

            if(i==wanderingTargets.Length){
                hasStartedWandering=false;
            }
        }
    }

    private void HandlePlayerTargeting(){
        target=playerTargets[0]; 
    }

    private bool IsInAttackRange(){
        //if(){
        //    return true;
        //}else{
        //    return false;
        //}

        return true;
    }
}
