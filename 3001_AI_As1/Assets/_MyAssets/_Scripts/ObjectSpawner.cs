using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectSpawnScript : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject player2Prefab;
    [SerializeField] GameObject enemy2Prefab;
    [SerializeField] GameObject player3Prefab;
    [SerializeField] GameObject enemy3Prefab;
    [SerializeField] GameObject player4Prefab;
    [SerializeField] GameObject enemy4Prefab;
    [SerializeField] GameObject avoidancePrefab;
    private GameObject currentEnemy;
    private GameObject currentPlayer;
    private GameObject currentPlayer2;
    private GameObject currentEnemy2;
    private GameObject currentPlayer3;
    private GameObject currentEnemy3;
    private GameObject currentPlayer4;
    private GameObject currentEnemy4;
    private GameObject currentAvoider;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(currentEnemy2);
            Destroy(currentPlayer2);
            Destroy(currentEnemy3);
            Destroy(currentPlayer3);
            Destroy(currentEnemy4);
            Destroy(currentPlayer4);
            Destroy(currentAvoider);

            if (currentEnemy != null)
            {
                Destroy(currentEnemy);
            }
            if (currentPlayer != null)
            {
                Destroy(currentPlayer);
            }
            currentEnemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);
            currentPlayer = Instantiate(playerPrefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);

            PlayerScript playerScript = currentPlayer.GetComponent<PlayerScript>();
            playerScript.Target = currentEnemy.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Destroy(currentEnemy);
            Destroy(currentPlayer);
            Destroy(currentEnemy3);
            Destroy(currentPlayer3);
            Destroy(currentEnemy4);
            Destroy(currentPlayer4);
            Destroy(currentAvoider);

            if (currentEnemy2 != null)
            {
                Destroy(currentEnemy2);
            }
            if (currentPlayer2 != null)
            {
                Destroy(currentPlayer2);
            }
            currentEnemy2 = Instantiate(enemy2Prefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);
            currentPlayer2 = Instantiate(player2Prefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);

            Player2Script player2Script = currentPlayer2.GetComponent<Player2Script>();
            player2Script.Target = currentEnemy2.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Destroy(currentEnemy);
            Destroy(currentPlayer);
            Destroy(currentEnemy2);
            Destroy(currentPlayer2);
            Destroy(currentEnemy4);
            Destroy(currentPlayer4);
            Destroy(currentAvoider);

            if (currentEnemy3 != null)
            {
                Destroy(currentEnemy3);
            }
            if (currentPlayer3 != null)
            {
                Destroy(currentPlayer3);
            }
            currentEnemy3 = Instantiate(enemy3Prefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);
            currentPlayer3 = Instantiate(player3Prefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);

            Player3Script player3Script = currentPlayer3.GetComponent<Player3Script>();
            player3Script.Target = currentEnemy3.transform;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Destroy(currentEnemy);
            Destroy(currentPlayer);
            Destroy(currentEnemy2);
            Destroy(currentPlayer2);
            Destroy(currentEnemy3);
            Destroy(currentPlayer3);

            if (currentEnemy4 != null)
            {
                Destroy(currentEnemy4);
            }
            if (currentPlayer4 != null)
            {
                Destroy(currentPlayer4);
            }
            if (currentAvoider != null)
            {
                Destroy(currentAvoider);
            }
            currentEnemy4 = Instantiate(enemy4Prefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);
            currentPlayer4 = Instantiate(player4Prefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f), Quaternion.identity);

            Player4Script player4Script = currentPlayer4.GetComponent<Player4Script>();
            player4Script.Target = currentEnemy4.transform;

            Vector3 midpoint = (currentPlayer4.transform.position + currentEnemy4.transform.position) / 2;
            currentAvoider = Instantiate(avoidancePrefab, midpoint, Quaternion.identity);           
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Destroy(currentEnemy);
            Destroy(currentPlayer);
            Destroy(currentEnemy2);
            Destroy(currentPlayer2);
            Destroy(currentEnemy3);
            Destroy(currentPlayer3);
            Destroy(currentEnemy4);
            Destroy(currentPlayer4);
            Destroy(currentAvoider);
        }
    }
}