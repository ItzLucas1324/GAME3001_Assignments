using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    [SerializeField] public float moveSpeed;
    [SerializeField] Transform agentRotationChild;
    [SerializeField] AgentAnimationController anController;
    int currentPathIndex = 0;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        anController = FindObjectOfType<AgentAnimationController>();
    }

    private void Update()
    {
        if (target != null)
        {
            FindPath(seeker.position, target.position);
            if (grid.path.Count > 0)
            {
                MoveAlongPath();
            }
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closeSet.Add(currentNode); 

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return 10 * (dstX + dstY);
       
    }

    void MoveAlongPath()
    {
        if (currentPathIndex < grid.path.Count)
        {
            Vector3 targetPosition = grid.path[currentPathIndex].worldPosition;

            seeker.position = Vector3.MoveTowards(seeker.position, targetPosition, moveSpeed * Time.deltaTime);

            Vector3 directionToTarget = targetPosition - seeker.position;

            if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y))
            {
                if (directionToTarget.x > 0)
                    directionToTarget = Vector3.right;
                else
                    directionToTarget = Vector3.left;
            }
            else
            {
                if (directionToTarget.y > 0)
                    directionToTarget = Vector3.up;
                else
                    directionToTarget = Vector3.down;
            }

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                agentRotationChild.rotation = targetRotation;
            }

            anController.UpdateAnimation(directionToTarget);

            if (Vector3.Distance(seeker.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }
}
