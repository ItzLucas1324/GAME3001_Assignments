using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectSpawnScript : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject player2Prefab;
    [SerializeField] GameObject enemy2Prefab;
    private GameObject currentEnemy;
    private GameObject currentPlayer;
    private GameObject currentPlayer2;
    private GameObject currentEnemy2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(currentEnemy2);
            Destroy(currentPlayer2);

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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Destroy(currentEnemy);
            Destroy(currentPlayer);
            Destroy(currentEnemy2);
            Destroy(currentPlayer2);
        }
    }
}