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
    int moveX;
    int moveY;
    bool isIdle = true;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        anController = FindObjectOfType<AgentAnimationController>();

        if (anController != null)
        {
            anController.SetIdleState();
            isIdle = true;
        }
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
            else if (!isIdle)
            {
                moveX = 0;
                moveY = 0;
                anController.UpdateAnimation(moveX, moveY);
                isIdle = true;
            }
        }
        else if (!isIdle)
        {
            moveX = 0;
            moveY = 0;
            anController.UpdateAnimation(moveX, moveY);
            isIdle = true;
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

            if (Vector3.Distance(seeker.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;

                if (currentPathIndex >= grid.path.Count)
                {
                    moveX = 0;
                    moveY = 0;
                    anController.UpdateAnimation(moveX, moveY);
                    isIdle = true;
                    return;
                }

                if (currentPathIndex < grid.path.Count)
                {
                    targetPosition = grid.path[currentPathIndex].worldPosition;
                }
            }

            seeker.position = Vector3.MoveTowards(seeker.position, targetPosition, moveSpeed * Time.deltaTime);

            Vector3 directionToTarget = targetPosition - seeker.position;

            bool needsUpdate = isIdle;
            int newMoveX = 0;
            int newMoveY = 0;

            if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y))
            {
                if (directionToTarget.x > 0)
                {
                    newMoveX = 1;
                    directionToTarget = Vector3.right;
                }
                else
                {
                    newMoveX = -1;
                    directionToTarget = Vector3.left;
                }
                newMoveY = 0;
            }
            else
            {
                if (directionToTarget.y > 0)
                {
                    newMoveY = 1;
                    directionToTarget = Vector3.up;
                }
                else
                {
                    newMoveY = -1;
                    directionToTarget = Vector3.down;
                }
                newMoveX = 0;
            }

            if (newMoveX != moveX || newMoveY != moveY || isIdle)
            {
                moveX = newMoveX;
                moveY = newMoveY;
                anController.UpdateAnimation(moveX, moveY);
                isIdle = false;
            }

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
                agentRotationChild.rotation = targetRotation;
            }
        }
        else if (!isIdle)
        {
            moveX = 0;
            moveY = 0;
            anController.UpdateAnimation(moveX, moveY);
            isIdle = true;
        }
    }
}