using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementHandler:MonoBehaviour{
    EnemyStats enemyStats;
    EnemySurroundingsCheck enemyDetection;

    Rigidbody rigidbody;

    NavMeshAgent agent;

    public Transform currentTarget;

    bool isInRange;

    [Header("Enemy Movement")]
    public EnemyMovementStates currentMovementState=EnemyMovementStates.SEEKINGTARGET;

    public enum EnemyMovementStates{
        IDLE,SEEKINGTARGET
    }

    private void Start(){
        enemyStats=GetComponent<EnemyStats>();
        agent=GetComponent<NavMeshAgent>();

        agent.speed=enemyStats.speed;
        agent.angularSpeed=enemyStats.turnSpeed;
        agent.acceleration=enemyStats.acceleration;
        agent.stoppingDistance=enemyStats.stoppingDst;

        currentTarget=this.transform;

        rigidbody=this.GetComponent<Rigidbody>();
    }

    void Update(){
        EnemyMovementStateMachine();
    }

    private void EnemyMovementStateMachine(){
        switch(currentMovementState){
            case EnemyMovementStates.IDLE:
            agent.SetDestination(this.transform.position);
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

    public void LookAtTarget(){
        Vector3 newPosition=currentTarget.position;
        newPosition.y=this.transform.position.y;

        this.transform.LookAt(newPosition);
    }

}
