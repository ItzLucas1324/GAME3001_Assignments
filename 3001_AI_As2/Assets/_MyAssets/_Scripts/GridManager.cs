using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileStatus
{
    UNVISITED,
    OPEN,
    CLOSED,
    IMPASSABLE,
    GOAL,
    START,
    PATH
};

public enum NeighbourTile
{
    TOP_TILE,
    RIGHT_TILE,
    BOTTOM_TILE,
    LEFT_TILE,
    NUM_OF_NEIGHBOUR_TILES
};

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tilePanelPrefab;
    [SerializeField] private GameObject panelParent;
    [SerializeField] AudioClip driving;
    [SerializeField] private Color[] colors;
    [SerializeField] private float baseTileCost = 1f;
    [SerializeField] private bool useManhattanHeuristic = true;
    private AudioSource aud;

    private GameObject[,] grid;
    private int rows = 12;
    private int columns = 16;
    private List<GameObject> obstacles = new List<GameObject>();

    public static GridManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        panelParent.gameObject.SetActive(false);
        ConnectGrid();

        aud = GetComponent<AudioSource>();
    }

    private void Initialize()
    {
        BuildGrid();
        DetectObstacles();
        ConnectGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            panelParent.gameObject.SetActive(!panelParent.gameObject.activeSelf);
            ConnectGrid();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            HighlightShortestPath();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found!");
                return;
            }
            Vector2 playerIndices = player.GetComponent<NavigationObject>().GetGridIndex();
            TileScript playerTile = grid[(int)playerIndices.y, (int)playerIndices.x].GetComponent<TileScript>();
            if (playerTile == null || playerTile.Node == null)
            {
                Debug.LogError("Player tile or its PathNode is null!");
                return;
            }
            PathNode start = playerTile.Node;

            GameObject finish = GameObject.FindGameObjectWithTag("Goal");
            if (finish == null)
            {
                Debug.LogError("Goal GameObject not found!");
                return;
            }
            Vector2 finishIndices = finish.GetComponent<NavigationObject>().GetGridIndex();
            TileScript finishTile = grid[(int)finishIndices.y, (int)finishIndices.x].GetComponent<TileScript>();
            if (finishTile == null || finishTile.Node == null)
            {
                Debug.LogError("Goal tile or its PathNode is null!");
                return;
            }
            PathNode goal = finishTile.Node;

            if (PathManager.Instance == null)
            {
                Debug.LogError("PathManager instance is null!");
                return;
            }

            PathManager.Instance.GetShortestPath(start, goal);

            List<PathConnection> computedPath = PathManager.Instance.path;
            if (computedPath != null && computedPath.Count > 0)
            {
                ShipPathFollower follower = player.GetComponent<ShipPathFollower>();
                if (follower != null)
                {
                    follower.SetPath(computedPath);
                }
                else
                {
                    Debug.LogError("PlayerPathFollower component missing on Ship.");
                }
            }
            else
            {
                Debug.Log("No valid path found.");
            }

            aud.PlayOneShot(driving);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetTileStatuses();
        }
    }

    public void HighlightShortestPath()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found!");
            return;
        }
        Vector2 playerIndices = player.GetComponent<NavigationObject>().GetGridIndex();
        TileScript playerTile = grid[(int)playerIndices.y, (int)playerIndices.x].GetComponent<TileScript>();

        GameObject finish = GameObject.FindGameObjectWithTag("Goal");
        if (finish == null)
        {
            Debug.LogError("Goal GameObject not found!");
            return;
        }
        Vector2 finishIndices = finish.GetComponent<NavigationObject>().GetGridIndex();
        TileScript finishTile = grid[(int)finishIndices.y, (int)finishIndices.x].GetComponent<TileScript>();

        if (playerTile == null || finishTile == null)
        {
            Debug.LogError("Player or Goal tile is missing!");
            return;
        }

        PathNode start = playerTile.Node;
        PathNode goal = finishTile.Node;

        if (PathManager.Instance == null)
        {
            Debug.LogError("PathManager instance is null!");
            return;
        }

        PathManager.Instance.GetShortestPath(start, goal);
        List<PathConnection> computedPath = PathManager.Instance.path;

        if (computedPath == null || computedPath.Count == 0)
        {
            Debug.Log("No valid path found.");
            return;
        }

        // Reset previous path tiles to unvisited
        foreach (GameObject tileObj in grid)
        {
            TileScript tileScript = tileObj.GetComponent<TileScript>();
            if (tileScript.status == TileStatus.PATH)
            {
                tileScript.SetStatus(TileStatus.UNVISITED);
            }
        }

        // Highlight new path
        foreach (PathConnection connection in computedPath)
        {
            TileScript tileScript = connection.toNode.tile.GetComponent<TileScript>();
            if (tileScript.status != TileStatus.START && tileScript.status != TileStatus.GOAL)
            {
                tileScript.SetStatus(TileStatus.PATH); // Change color/status to indicate the path
            }
        }
    }



    private void BuildGrid()
    {
        grid = new GameObject[rows, columns];
        int count = 0;
        float rowPos = 5.5f;

        for (int row = 0; row < rows; row++, rowPos--)
        {
            float colPos = -7.5f;
            for (int col = 0; col < columns; col++, colPos++)
            {
                GameObject tileInst = Instantiate(tilePrefab, new Vector3(colPos, rowPos, 0f), Quaternion.identity);
                TileScript tileScript = tileInst.GetComponent<TileScript>();
                if (tileScript == null)
                {
                    Debug.LogError("TileScript missing on tile prefab!");
                    continue;
                }
                tileScript.SetColor(colors[System.Convert.ToInt32((count++ % 2 == 0))]);
                tileInst.transform.parent = transform;
                grid[row, col] = tileInst;

                GameObject panelInst = Instantiate(tilePanelPrefab, tilePanelPrefab.transform.position, Quaternion.identity);
                panelInst.transform.SetParent(panelParent.transform);
                RectTransform panelTransform = panelInst.GetComponent<RectTransform>();
                panelTransform.localScale = Vector3.one;
                panelTransform.anchoredPosition = new Vector3(64f * col, -64f * row);
                tileScript.tilePanel = panelInst.GetComponent<TilePanelScript>();

                tileScript.Node = new PathNode(tileInst);
            }
            count--;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerIndices = player.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)playerIndices.y, (int)playerIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.START);
        }
        else
        {
            Debug.LogError("Player GameObject not found during grid build.");
        }

        GameObject finish= GameObject.FindGameObjectWithTag("Goal");
        if (finish != null)
        {
            Vector2 finishIndices = finish.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)finishIndices.y, (int)finishIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
            SetTileCosts(finishIndices);
        }
        else
        {
            Debug.LogError("Goal GameObject not found during grid build.");
        }
    }

    public void ConnectGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();
                tileScript.ResetNeighbourConnections();

                if (tileScript.status == TileStatus.IMPASSABLE)
                {
                    continue;
                }

                if (row > 0 && grid[row - 1, col].GetComponent<TileScript>().status != TileStatus.IMPASSABLE)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.TOP_TILE, grid[row - 1, col]);
                    tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row - 1, col].GetComponent<TileScript>().Node,
                        Vector3.Distance(tileScript.transform.position, grid[row - 1, col].GetComponent<TileScript>().transform.position)));
                }
                if (col < columns - 1 && grid[row, col + 1].GetComponent<TileScript>().status != TileStatus.IMPASSABLE)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.RIGHT_TILE, grid[row, col + 1]);
                    tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row, col + 1].GetComponent<TileScript>().Node,
                        Vector3.Distance(tileScript.transform.position, grid[row, col + 1].GetComponent<TileScript>().transform.position)));
                }
                if (row < rows - 1 && grid[row + 1, col].GetComponent<TileScript>().status != TileStatus.IMPASSABLE)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.BOTTOM_TILE, grid[row + 1, col]);
                    tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row + 1, col].GetComponent<TileScript>().Node,
                        Vector3.Distance(tileScript.transform.position, grid[row + 1, col].GetComponent<TileScript>().transform.position)));
                }
                if (col > 0 && grid[row, col - 1].GetComponent<TileScript>().status != TileStatus.IMPASSABLE)
                {
                    tileScript.SetNeighbourTile((int)NeighbourTile.LEFT_TILE, grid[row, col - 1]);
                    tileScript.Node.AddConnection(new PathConnection(tileScript.Node, grid[row, col - 1].GetComponent<TileScript>().Node,
                        Vector3.Distance(tileScript.transform.position, grid[row, col - 1].GetComponent<TileScript>().transform.position)));
                }
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return grid;
    }

    public Vector2 GetGridPosition(Vector2 worldPosition)
    {
        float xPos = Mathf.Floor(worldPosition.x) + 0.5f;
        float yPos = Mathf.Floor(worldPosition.y) + 0.5f;
        return new Vector2(xPos, yPos);
    }

    public void SetTileCosts(Vector2 targetIndices)
    {
        float distance = 0f;
        float dx = 0f;
        float dy = 0f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                TileScript tileScript = grid[row, col].GetComponent<TileScript>();

                if (useManhattanHeuristic)
                {
                    dx = Mathf.Abs(col - targetIndices.x);
                    dy = Mathf.Abs(row - targetIndices.y);
                    distance = dx + dy;
                }
                else
                {
                    dx = targetIndices.x - col;
                    dy = targetIndices.y - row;
                    distance = Mathf.Sqrt(dx * dx + dy * dy);
                }

                float adjustedCost = distance * baseTileCost;
                tileScript.cost = adjustedCost;
                tileScript.tilePanel.costText.text = tileScript.cost.ToString("F1");
            }
        }
    }

    public void SetTileStatuses()
    {
        foreach (GameObject go in grid)
        {
            go.GetComponent<TileScript>().SetStatus(TileStatus.UNVISITED);
        }

        foreach (GameObject obstacle in obstacles)
        {
            Vector2 obstacleIndex = obstacle.GetComponent<NavigationObject>().GetGridIndex();
            grid[(int)obstacleIndex.y, (int)obstacleIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerIndices = player.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)playerIndices.y, (int)playerIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.START);

        GameObject finish = GameObject.FindGameObjectWithTag("Goal");
        Vector2 finishIndices = finish.GetComponent<NavigationObject>().GetGridIndex();
        grid[(int)finishIndices.y, (int)finishIndices.x].GetComponent<TileScript>().SetStatus(TileStatus.GOAL);
    }

    private void DetectObstacles()
    {
        GameObject[] placedObstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach (GameObject obstacle in placedObstacles)
        {
            NavigationObject navObstacle = obstacle.GetComponent<NavigationObject>();
            if (navObstacle != null)
            {
                Vector2 obstacleIndex = navObstacle.GetGridIndex();
                grid[(int)obstacleIndex.y, (int)obstacleIndex.x].GetComponent<TileScript>().SetStatus(TileStatus.IMPASSABLE);
                obstacles.Add(obstacle);
            }
        }
    }
}
