using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{
    System.Random rand = new System.Random();
    NavMeshAgent agent;

    public Transform pathHolder;
    public float viewDistance = 20;

    private int pathIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        Vector3[] path = new Vector3[pathHolder.childCount];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = pathHolder.GetChild(i).position;
            //path[i] = new Vector3(path[i].x, transform.position.y, path[i].z);
        }

        StartCoroutine(Patrol(path));
    }

    IEnumerator Patrol(Vector3[] path)
    {
        while (true)
        {
            // Check if we've reached the destination
            if (agent.velocity.sqrMagnitude <= 0.1)
            {
                if (pathIndex >= path.Length)
                {
                    pathIndex = 0;
                }

                agent.SetDestination(path[pathIndex]);

                pathIndex++;
                yield return new WaitForSeconds(1);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    /*
    Vector3 RandomPoint()
    {
        int x = rand.Next((int)minX, (int)maxX);
        int z = rand.Next((int)minZ, (int)maxZ);

        return GetNavPosition(new Vector3(x, 0, z));
    }

    private Vector3 GetNavPosition(Vector3 center)
    {
        // Get Nearest Point on NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(center, out hit, 30.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        throw new ArgumentException("Failed to Get NavMesh Point");
    }
    */

    void OnDrawGizmos()
    {
        // Waypoints
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(waypoint.position, previousPosition);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);

        // View Distance
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
