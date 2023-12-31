using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThres = .5f;
    
    public Transform target;
    public float speed = 20;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 10;
    
    Path path;

    void Start()
    {
        StartCoroutine(UpdatePath());
    }
    
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        
        if(Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position,OnPathFound);
        
        float sqrMoveThres = pathUpdateMoveThres * pathUpdateMoveThres;
        Vector3 targetPosOld = target.position;
        
        while(true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if((target.position - targetPosOld).sqrMagnitude > sqrMoveThres)
            {
                PathRequestManager.RequestPath(transform.position, target.position,OnPathFound);
                targetPosOld = target.position;
            }
        }
    }
 
    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);
        
        float speedPercent = 1;

        while(followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            while(path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if(pathIndex == path.finishlineIndex)
                {
                    followingPath = false;
                    break;
                }else{
                    pathIndex++;
                }

            }

            if(followingPath)
            {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishlineIndex].DistanceFromPoint(pos2D)/ stoppingDst);

                    Quaternion targetRotation = Quaternion.LookRotation (path.lookPoints [pathIndex] - transform.position);
				    transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
				    transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                    
            }
            
            yield return null;
        }
               
    }
    

    public void OnDrawGizmos(){
		if (path != null) {
			path.DrawWithGizmos();
		}
	}
} 
