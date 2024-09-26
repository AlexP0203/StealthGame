using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ears : MonoBehaviour
{
    [SerializeField] GameObject playerRef;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] float radius;
    [Range(0, 360)][SerializeField] float angle;

    bool canHearPlayer;
    void Start()

    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {

    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }

    }

    

    private void FieldOfViewCheck()
    {
        if (!canHearPlayer)
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            canHearPlayer = true;
                            gameObject.transform.parent.GetComponent<Guard>().Investigate();
                            StartCoroutine(earsRestart());
                        }
                    }
                    else
                    {
                        canHearPlayer = false;
                    }
                }
                else
                {
                    canHearPlayer = false;
                }
            }
            else if (canHearPlayer)
            {
                canHearPlayer = false;
            }
        }
        
    }

    private IEnumerator earsRestart()
    {
        yield return new WaitForSeconds(10.0f);
        canHearPlayer = false;

    }

    public float getRadius()
    {
        return radius;
    }

    public float getAngle()
    {
        return angle;
    }

    public void setRadius(float x)
    {
        radius = 20;
    }
}
