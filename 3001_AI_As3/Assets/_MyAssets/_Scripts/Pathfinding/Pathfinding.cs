using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    [SerializeField] public float moveSpeed;
    [SerializeField] Transform agentRotationChild;
    int currentPathIndex = 0;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
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

            // Move the seeker along the path
            seeker.position = Vector3.MoveTowards(seeker.position, targetPosition, moveSpeed * Time.deltaTime);

            // Calculate the direction to the next target position
            Vector3 directionToTarget = targetPosition - seeker.position;

            // Snap the movement to the closest cardinal direction (up, down, left, right)
            if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y))  // Move horizontally (left or right)
            {
                if (directionToTarget.x > 0)
                    directionToTarget = Vector3.right;  // Move right
                else
                    directionToTarget = Vector3.left;  // Move left
            }
            else  // Move vertically (up or down)
            {
                if (directionToTarget.y > 0)
                    directionToTarget = Vector3.up;  // Move up
                else
                    directionToTarget = Vector3.down;  // Move down
            }

            // If the agent is moving, rotate the child object to face the movement direction
            if (directionToTarget != Vector3.zero)
            {
                // Rotate the agentRotationChild so that its up vector aligns with the directionToTarget
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                agentRotationChild.rotation = targetRotation;
            }

            if (Vector3.Distance(seeker.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }
}
