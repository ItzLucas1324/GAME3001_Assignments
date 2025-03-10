using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    private GridManager gridManager;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject goalPrefab;

    void Start()
    {
        // Attempt to find the GridManager if it's not assigned in the Inspector
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
            if (gridManager == null)
            {
                Debug.LogError("GridManager not found in the scene!");
            }
        }
    }

    void Update()
    {
        // Handle left-click for spawning the player
        if (Input.GetMouseButtonDown(0))  // Left-click
        {
            SpawnAgent("Player");
        }

        // Handle right-click for spawning the goal
        if (Input.GetMouseButtonDown(1))  // Right-click
        {
            SpawnAgent("Goal");
        }
    }

    private void SpawnAgent(string tag)
    {
        // Delete the existing agent with the specified tag
        DeleteExistingObjects(tag);

        // Get the mouse position in world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; // Set z to 0 since it's 2D

        // Instantiate the correct prefab based on the tag
        GameObject prefabToSpawn = null;

        if (tag == "Player")
        {
            prefabToSpawn = playerPrefab;
        }
        else if (tag == "Goal")
        {
            prefabToSpawn = goalPrefab;
        }

        if (prefabToSpawn != null)
        {
            GameObject spawnedAgent = Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);
            spawnedAgent.tag = tag;  // Ensure correct tag for each spawned object

            // Update GridManager with the new positions
            gridManager.SetTileStatuses();
            gridManager.ConnectGrid(); // Recalculate the grid connections
        }
    }

    private void DeleteExistingObjects(string tag)
    {
        // Find and destroy the existing object with the specified tag
        GameObject existingObject = GameObject.FindGameObjectWithTag(tag);
        if (existingObject != null)
        {
            Destroy(existingObject);
        }
    }
}
