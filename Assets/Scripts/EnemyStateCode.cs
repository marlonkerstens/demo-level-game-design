using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateCode : MonoBehaviour
{
    private GameObject[] wayPoints;
    public GameObject player;
    private NavMeshAgent navMeshAgent;
    private Vector3 targetWpLocation;
    private Vector3 lastLocation;
    private Vector3 secondToLastLocation;

    private int followDistance = 10;
    private int attackDistance = 1;




    // Update is called once per frame
    void Awake()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("EnemyWayPoint");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    public void Enter_Patrol()
    {
        determineNextWaypoint();
    }
    public void Update_Patrol()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= followDistance)
        {
            CustomEvent.Trigger(this.gameObject,"isFollowing");
        }

    }
    public void FixedUpdate_Patrol()
    {
        transform.rotation.SetLookRotation(targetWpLocation * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetWpLocation) <= 1.5f)
        {
            secondToLastLocation = lastLocation;
            lastLocation = targetWpLocation;
            determineNextWaypoint();
        }
    }
    public void Exit_Patrol()
    {
    }

    public void Enter_Follow()
    {

    }
    public void Update_Follow()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            CustomEvent.Trigger(this.gameObject,"isAttacking");
        }
        if (Vector3.Distance(transform.position, player.transform.position) > followDistance)
        {
            CustomEvent.Trigger(this.gameObject,"isPatrolling");
        }
        
    }
    public void FixedUpdate_Follow()
    {
        navMeshAgent.SetDestination(player.transform.position);
    }
    public void Exit_Follow()
    {
 
    }
    
    public void Enter_Blind()
    {
        navMeshAgent.speed = 0;
        StartCoroutine(WaitPatrolling());
    }
    public void Update_Blind()
    {
   
    }
    public void FixedUpdate_Blind()
    {
    }
    public void Exit_Blind()
    {
        navMeshAgent.speed = 2;
    }
    public void Enter_Attack()
    {

    }
    public void Update_Attack()
    {
        Debug.Log("Attacking");
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            CustomEvent.Trigger(this.gameObject,"isFollowing");
        }
    }
    public void FixedUpdate_Attack()
    {
        navMeshAgent.SetDestination(player.transform.position);
    }
    public void Exit_Attack()
    {
        
    }

    private void determineNextWaypoint()
    {
        GameObject closestWP = null;
        foreach (GameObject wp in wayPoints)
        {
            if (wp.transform.position != lastLocation && wp.transform.position != secondToLastLocation)
            {
                if (closestWP == null)
                {
                    closestWP = wp;
                }
                else
                {
                    if (Vector3.Distance(transform.position, wp.transform.position) < (Vector3.Distance(transform.position, closestWP.transform.position)))
                    {
                        closestWP = wp;
                    }
                }
            }
        }
        targetWpLocation = closestWP.transform.position;
        navMeshAgent.SetDestination(targetWpLocation);
    }
    IEnumerator WaitPatrolling()
    {
        yield return new WaitForSeconds(3);
        CustomEvent.Trigger(this.gameObject,"isPatrolling");
    }
    
}
