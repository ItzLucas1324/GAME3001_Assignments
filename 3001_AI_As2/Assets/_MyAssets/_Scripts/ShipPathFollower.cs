using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ShipPathFollower : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float waypointThreshold = 0.1f;
    [SerializeField] AudioClip achievment;
    private AudioSource aud;

    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypointIndex = 0;
    private bool followingPath = false;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    public void SetPath(List<PathConnection> pathConnections)
    {
        waypoints.Clear();
        if (pathConnections == null || pathConnections.Count == 0)
        {
            Debug.Log("No path connections provided.");
            return;
        }

        foreach (var connection in pathConnections)
        {
            Vector3 pos = connection.toNode.tile.transform.position;
            waypoints.Add(pos);
        }
        currentWaypointIndex = 0;
        followingPath = true;
        Debug.Log("New path set. Waypoints count: " + waypoints.Count);
    }

    void Update()
    {
        if (followingPath && waypoints.Count > 0)
        {
            Vector3 targetPos = waypoints[currentWaypointIndex];
            Vector3 direction = targetPos - transform.position;
            float distanceToWaypoint = direction.magnitude;

            if (distanceToWaypoint > 0.001f)
            {
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
                transform.eulerAngles = new Vector3(0, 0, newAngle);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < waypointThreshold)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    followingPath = false;
                    transform.position = targetPos;

                    aud.PlayOneShot(achievment);
                    GetComponent<NavigationObject>().SetGridIndex();
                    Debug.Log("Player reached destination and grid index updated.");
                }
            }
        }
    }
}