using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AgentObject
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float waypointThreshold = 0.1f;
    private Rigidbody2D rb;
    private List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypointIndex = 0;
    private bool followingPath = false;

    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PathManager.Instance != null && PathManager.Instance.path != null && PathManager.Instance.path.Count > 0 && !followingPath)
        {
            waypoints.Clear();
            foreach (var connection in PathManager.Instance.path)
            {
                Vector3 pos = connection.toNode.tile.transform.position;
                waypoints.Add(pos);
            }
            currentWaypointIndex = 0;
            followingPath = true;
            Debug.Log("Path computed. Waypoints count: " + waypoints.Count);
        }

        if (followingPath && waypoints.Count > 0)
        {
            Vector3 targetPos = waypoints[currentWaypointIndex];
            Vector3 direction = (targetPos - transform.position).normalized;


            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, newAngle);
            rb.velocity = transform.up * movementSpeed;

            if (Vector3.Distance(transform.position, targetPos) < waypointThreshold)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    followingPath = false;
                    rb.velocity = Vector2.zero;
                    Debug.Log("Player reached the destination.");
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}