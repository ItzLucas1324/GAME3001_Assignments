using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnScript : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject playerPrefab;
    private GameObject currentEnemy;
    private GameObject currentPlayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Destroy(currentEnemy);
            Destroy(currentPlayer);
        }
    }
}
