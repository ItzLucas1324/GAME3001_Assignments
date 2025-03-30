using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    private Pathfinding pathfindingScript;
    private Transform playerTransform;

    private void Awake()
    {
        pathfindingScript = FindObjectOfType<Pathfinding>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartChase()
    {
        pathfindingScript.target = playerTransform;
    }
}
