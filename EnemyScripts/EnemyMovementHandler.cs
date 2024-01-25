using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementHandler:MonoBehaviour{
    EnemyStats enemyStats;
    EnemySurroundingsCheck enemyDetection;

    Rigidbody rigidbody;

    Transform currentTarget;

    public NavMeshAgent agent;

    bool isInRange;

    [Header("Enemy Movement")]
    public EnemyMovementStates currentMovementState=EnemyMovementStates.SEEKINGTARGET;

    public enum EnemyMovementStates{
        IDLE,SEEKINGTARGET
    }

    private void Start(){
        enemyStats=GetComponent<EnemyStats>();
        enemyDetection=this.transform.GetChild(0).GetComponent<EnemySurroundingsCheck>();
        agent=GetComponent<NavMeshAgent>();

        agent.speed=enemyStats.speed;
        agent.angularSpeed=enemyStats.turnSpeed;
        agent.acceleration=enemyStats.acceleration;
        agent.stoppingDistance=enemyStats.stoppingDst;

        rigidbody=this.GetComponent<Rigidbody>();
    }

    void Update(){
        EnemyMovementStateMachine();
    }

    public void EnemyMovementStateMachine(){
        switch(currentMovementState){
            case EnemyMovementStates.IDLE:
            break;
            case EnemyMovementStates.SEEKINGTARGET:
            agent.SetDestination(currentTarget.position);
            break;
        }
    }

    public void StateToIdle(){
        currentMovementState=EnemyMovementStates.IDLE;
    }

    public void StateToSeeking(){
        currentMovementState=EnemyMovementStates.SEEKINGTARGET;
    }

    public void SetTarget(Transform target){
        currentTarget=target;
    }

}
