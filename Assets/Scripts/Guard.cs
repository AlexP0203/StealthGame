using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum enemyStates
{
    PATROL,
    INVESTIGATE,
    CHASE
}

public class Guard : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] GameObject Ears;
    [SerializeField] GameObject Eyes;
   

    Rigidbody rb;
    NavMeshPath navPath;
    Queue<Vector3> remainingPoints;
    Vector3 currentTargetPoint;
    enemyStates enemyState = enemyStates.PATROL;
    Vector3[] patrolArray;
    float distToPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        navPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();
        patrolArray = new[] { new Vector3(3f, 0.5f, 20f), new Vector3(3f, 0.5f, 30f), new Vector3(17f, 0.5f, 30f), new Vector3(17f, 0.5f, 20f) };
       

        Patrol();
    }

    void Update()
    {
        //agent.SetDestination(target.position);

        var new_forward = (currentTargetPoint - transform.position).normalized;
        new_forward.y = 0;
        transform.forward = new_forward;



        distToPoint = Vector3.Distance(transform.position, currentTargetPoint);

        if (distToPoint < 1)
        {

            if (remainingPoints.Count > 0)
            {
                currentTargetPoint = remainingPoints.Dequeue();

            }
            else
            {
                switch (enemyState)
                {
                    case enemyStates.PATROL:
                        Patrol();
                        break;
                    case enemyStates.INVESTIGATE:
                        Investigate();
                        break;
                    case enemyStates.CHASE:
                        Chase();
                        break;
                }
            }
        }

        
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnDrawGizmos()
    {
        if (navPath == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        foreach (Vector3 node in navPath.corners)
        {
            Gizmos.DrawWireSphere(node, 1.0f);
        }
    }


    public void Patrol()
    {
        Debug.Log("Patrol");
        speed = 2f;
        enemyState = enemyStates.PATROL;
        remainingPoints.Clear();

        foreach (Vector3 p in patrolArray)
        {
            remainingPoints.Enqueue(p);
        }

        currentTargetPoint = remainingPoints.Dequeue();
    }

    public void Investigate()
    {
        //StartCoroutine(invToPatrol());
        if (!(enemyState == enemyStates.INVESTIGATE) && !(enemyState == enemyStates.CHASE))
        {
            StartCoroutine(Pause());
        }
        //enemyState = enemyStates.INVESTIGATE;
        if(!(enemyState == enemyStates.CHASE))
        {
            Debug.Log("Investigate");
            remainingPoints.Clear();
            if (agent.CalculatePath(target.position, navPath))
            {
                foreach (Vector3 p in navPath.corners)
                {
                    remainingPoints.Enqueue(p);
                }

                currentTargetPoint = remainingPoints.Dequeue();
            }
        }
        
    }

    public void Chase()
    {
        Debug.Log("chase");
        speed = 4.0f;
        remainingPoints.Clear();
        enemyState = enemyStates.CHASE;
        //StartCoroutine(chaseToPatrol());
        if (agent.CalculatePath(target.position, navPath))
        {
            foreach (Vector3 p in navPath.corners)
            {
                remainingPoints.Enqueue(p);
            }

            currentTargetPoint = remainingPoints.Dequeue();
        }
    }


    IEnumerator invToPatrol()
    {
        yield return new WaitForSeconds(7.0f);
        if (distToPoint > 3)
        {
            enemyState = enemyStates.PATROL;
        }
    }

    IEnumerator chaseToPatrol()
    {
        Ears.SetActive(true);
        yield return new WaitForSeconds(10.0f);
        if (distToPoint > 8)
        {
            enemyState = enemyStates.PATROL;
        }
        Ears.SetActive(false);
    }

    IEnumerator Pause()
    {
        speed = 0;
        yield return new WaitForSeconds(5.0f);
        speed = 3;
    }


    private void OnCollisionEnter(Collision other)
    {
        SceneManager.LoadScene("Recap");
    }
}
