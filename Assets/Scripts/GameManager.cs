using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    float timeUntilNextEnemy = 2.0f;
    float lastEnemyTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastEnemyTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // every 2 seconds, instantiate a new enemy prefab and position at the edge
        // of the viewport;
        if (Time.time - lastEnemyTime > timeUntilNextEnemy)
        {
            lastEnemyTime = Time.time;

            GameObject enemy = Instantiate(enemyPrefab as GameObject);

            // random integer between 0 and 3
            int side = Random.Range(0, 4);
            switch (side)
            {
                case 0:
                    // top
                    enemy.transform.position = Camera.main.ViewportToWorldPoint(
                        new Vector3(Random.Range(0.0f, 1.0f), 1.0f, 0.0f)
                    );
                    break;
                case 1:
                    // right
                    enemy.transform.position = Camera.main.ViewportToWorldPoint(
                        new Vector3(1.0f, Random.Range(0.0f, 1.0f), 0.0f)
                    );
                    break;
                case 2:
                    // bottom
                    enemy.transform.position = Camera.main.ViewportToWorldPoint(
                        new Vector3(Random.Range(0.0f, 1.0f), 0.0f, 0.0f)
                    );
                    break;
                case 3:
                    // left
                    enemy.transform.position = Camera.main.ViewportToWorldPoint(
                        new Vector3(0.0f, Random.Range(0.0f, 1.0f), 0.0f)
                    );
                    break;
            }
        }
    }
}
