using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour
{
    System.Random rand = new System.Random();
    private NavMeshAgent agent;
    private Transform player;
    private Light spotLight;

    public Transform pathHolder;
    public float viewDistance = 50;
    public float viewAngle = 70;

    private LayerMask obstacleMask;
    private int pathIndex = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        spotLight = GetComponentInChildren<Light>();

        obstacleMask = LayerMask.GetMask("Obstacle");

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
            // Look for player
            if (CanSeePlayer())
            {
                spotLight.color = Color.red;
            }
            else
            {
                spotLight.color = Color.green;
            }

            // Check if we've reached the destination
            if (agent.velocity.sqrMagnitude <= 0.1)
            {
                if (pathIndex >= path.Length)
                {
                    pathIndex = 0;
                }

                agent.SetDestination(path[pathIndex]);

                pathIndex++;
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    bool CanSeePlayer()
    {
        // Get View distance from visibility level
        //float viewDistance = visibilityLevelToViewDistanceMap[playerStats.GetVisibility()];
        float viewDistance = 15;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Within View Distance
        if (distanceToPlayer > viewDistance)
        {
            return false;
        }

        // Line of Sight not blocked by obstacles
        if (Physics.Linecast(transform.position, player.position, obstacleMask))
        {
            return false;
        }

        // Within View Angle
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Math.Abs(Vector3.Angle(transform.forward, dirToPlayer));
        if (angleToPlayer > viewAngle)
        {
            return false;
        }

        /*
        // Line of Sight not blocked by heavy cover
        // Treat this the same as the lowest visibility for this time of day
        float lowestViewDistance = visibilityLevelToViewDistanceMap[playerStats.GetObscuredVisibility()];
        if (Physics.Linecast(transform.position, player.position, heavyCoverMask) && distanceToPlayer > lowestViewDistance)
        {
            return false;
        }
        */

        return true;
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
