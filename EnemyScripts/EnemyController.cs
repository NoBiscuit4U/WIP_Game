using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController:MonoBehaviour{
    EnemyStats enemyStats;
    EnemyMovementHandler enemyMovement;
    EnemySurroundingsCheck enemyDetection;
    PlayerController player;

    bool hasStartedSetWandering=false;
    bool hasStartedRandomWandering=false;
    public bool isAttackingPlayer;

    [Header("Targets and CurrentPos")]
	public Transform[] playerTargets;
	public Transform[] setWanderingTargets;
    public Transform randomWanderingTarget;
    public Transform enemyCurrentPos;
	public Transform target;

    [Header("Enemy Aggro State")]
    public EnemyAggroStates currentAggroState;

    public enum EnemyAggroStates{
        DORMANT,WANDERING,SEEKING
    }

    [Header("Enemy Effects")]
    public GameObject onDeathEffect;

    private void Start(){  
        enemyStats=this.GetComponent<EnemyStats>();
        enemyMovement=this.GetComponent<EnemyMovementHandler>();
        enemyDetection=this.GetComponent<EnemySurroundingsCheck>();
    }

    private void Update(){
        if(enemyStats.HP<=0){
            HandleDeath();
        }

        if(isAttackingPlayer&&IsInAttackRange()){
            player=target.transform.GetComponent<PlayerController>();
            player.ChangeHP(10);
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
            isAttackingPlayer=false;
            enemyMovement.StateToIdle();
            break;
            case EnemyAggroStates.WANDERING:
            isAttackingPlayer=false;
            enemyMovement.StateToSeeking();
            enemyMovement.SetTarget(target);
            StartRandomWanderingRoutine();
            break;
            case EnemyAggroStates.SEEKING:
            HandlePlayerTargeting();
            isAttackingPlayer=true;
            enemyMovement.StateToSeeking();
            enemyMovement.SetTarget(target);
            break;
        }
    }
    
    private void StartSetWanderingRoutine(bool stop){
        if(hasStartedSetWandering==false){
            hasStartedSetWandering=true;
            target=randomWanderingTarget;
            StartCoroutine(EnemyFollowSetPath());
        }

        if(stop==true){
            hasStartedSetWandering=false;
            StopCoroutine(EnemyFollowSetPath());
        }
    }

    private void StartRandomWanderingRoutine(){
        if(!hasStartedRandomWandering){
            hasStartedRandomWandering=true;
            StartCoroutine(SetNewWanderingTarget(100));
            target=randomWanderingTarget;
        }
    }

    private IEnumerator EnemyFollowSetPath(){
        yield return new WaitForSeconds(0.01f);
        for(int i=0;i<setWanderingTargets.Length;){
            target=setWanderingTargets[i];

            if(enemyCurrentPos.position.x==setWanderingTargets[i].position.x&&enemyCurrentPos.position.z==setWanderingTargets[i].position.z){
                i++;
            }

            if(i==setWanderingTargets.Length){
                hasStartedSetWandering=false;
            }
        }
    }

    private IEnumerator SetNewWanderingTarget(float newTargetRange){
		bool isWalkable=false;

		randomWanderingTarget.position=NewWanderingTargetPos(newTargetRange);

		while(!isWalkable){
			Vector3 newPos=NewWanderingTargetPos(newTargetRange);
			bool checkIfWalkable=Physics.Raycast(newPos,Vector3.down,2f,3);
			yield return new WaitForSeconds(0.01f);

			if(checkIfWalkable){
				randomWanderingTarget.transform.position=newPos;
				isWalkable=true;
                hasStartedRandomWandering=false;
				StopCoroutine(SetNewWanderingTarget(newTargetRange));
			}
		}
	}

    public Vector3 NewWanderingTargetPos(float newTargetRange){
        float randomTargetX=Random.Range(-newTargetRange,newTargetRange); 
        float randomTargetY=Random.Range(-newTargetRange,newTargetRange);
        float randomTargetZ=Random.Range(-newTargetRange,newTargetRange); 

        Vector3 newTargetPos=new Vector3(enemyCurrentPos.position.x+randomTargetX,enemyCurrentPos.position.y,enemyCurrentPos.position.z+randomTargetZ);

        return newTargetPos;
    }

    private void HandlePlayerTargeting(){
        target=playerTargets[0]; 
    }

    private bool IsInAttackRange(){
        bool returnValue=false; 
        if(Mathf.Abs(enemyCurrentPos.position.x)<=Mathf.Abs(target.position.x)+enemyStats.attackRange&&
                                                  Mathf.Abs(enemyCurrentPos.position.y)<=Mathf.Abs(target.position.y)+enemyStats.attackRange&&isPlayerVisible()){
            returnValue=true;
        }

        return returnValue;
    }

    public bool isPlayerVisible(){
        return enemyDetection.IsPlayerInView(target);
    }
}
