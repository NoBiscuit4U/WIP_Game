using UnityEngine;
using System.Collections;

public class EnemyPathFinder:MonoBehaviour {
	const float minPathUpdateTime=.2f;
	const float pathUpdateMoveThreshold=.5f;

	bool followPath;

	EnemyStats enemyStats;
	EnemyController enemyController;

	Path path;

    [Header("Targets and CurrentPos")]
	public Transform[] playerTargets;
	public Transform wanderingTarget;
	public Transform target;
	public Transform enemyCurrentPos;

	[Header("Enemy Move Stats")]
	public float speed;
	public float turnSpeed;
	public float turnDst;
	public float stoppingDst;

	void Start(){
		enemyStats=GetComponent<EnemyStats>();
		enemyController=GetComponent<EnemyController>();

		speed=enemyStats.speed;
		turnSpeed=enemyStats.turnSpeed;
		turnDst=enemyStats. turnDst;
		stoppingDst=enemyStats.stoppingDst;       
		enemyCurrentPos=this.transform;

		StartCoroutine(SetNewWanderingTarget(30));
	}

	void Update(){
		StartCoroutine(UpdatePath());

		if(target.gameObject.layer==LayerMask.NameToLayer("Player")){
			stoppingDst=1;
		}
	}

	public void OnPathFound(Vector3[] waypoints,bool pathSuccessful){
		if(pathSuccessful){
			path=new Path(waypoints,transform.position,turnDst,stoppingDst);

			StartCoroutine(SetNewWanderingTarget(30));
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator UpdatePath() {
		if(Time.timeSinceLevelLoad<.3f){
			yield return new WaitForSeconds (.3f);
		}
		PathRequestManager.RequestPath(new PathRequest(transform.position,target.position,OnPathFound));

		float sqrMoveThreshold=pathUpdateMoveThreshold*pathUpdateMoveThreshold;
		Vector3 targetPosOld=target.position;

		while (true) {
			yield return new WaitForSeconds(minPathUpdateTime);
			//print (((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
			if ((target.position-targetPosOld).sqrMagnitude>sqrMoveThreshold) {
				PathRequestManager.RequestPath(new PathRequest(transform.position,target.position,OnPathFound));
				targetPosOld=target.position;
			}
		}
	}

	IEnumerator FollowPath() {
		bool followingPath=true;
		int pathIndex=0;
		transform.LookAt(path.lookPoints[0]);

		float speedPercent=1;

		while(followingPath){
			Vector2 pos2D=new Vector2(transform.position.x,transform.position.z);
			while(path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)){
				if(pathIndex==path.finishLineIndex){
					followingPath=false;
					break;
				}else{
					pathIndex++;
				}
			}

			if(followingPath){
				if(pathIndex>=path.slowDownIndex&&stoppingDst>0){
					speedPercent=Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D)/stoppingDst);
					if(speedPercent<0.01f){
						followingPath=false;
					}
				}

				Quaternion targetRotation=Quaternion.LookRotation(path.lookPoints[pathIndex]-transform.position);
				transform.rotation=Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime*turnSpeed);
				transform.Translate(Vector3.forward*Time.deltaTime*speed*speedPercent,Space.Self);
			}

			yield return null;

		}
	}

	IEnumerator SetNewWanderingTarget(float newTargetRange){
		bool isWalkable=false;

		wanderingTarget.position=NewWanderingTargetPos(newTargetRange);

		while(!isWalkable){
			Vector3 newPos=NewWanderingTargetPos(newTargetRange);
			bool checkIfWalkable=Physics.Raycast(newPos,Vector3.down,2f,3);
			yield return new WaitForSeconds(0.01f);

			if(checkIfWalkable){
				wanderingTarget.transform.position=newPos;
				isWalkable=true;
				StopCoroutine("SetNewWanderingTarget");
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

	public void OnDrawGizmos(){
		if(path!=null) {
			path.DrawWithGizmos ();
		}
	}
}
