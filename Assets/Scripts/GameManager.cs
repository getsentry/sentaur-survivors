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

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab as GameObject);

        // viewport coords:
        //   (0, 0) is bottom left
        //   (1, 1) is top right
        var spawnViewportCoord = new Vector3(0.0f, 0.0f, 0.0f);

        // randomly choose a side to spawn on, then randomly choose a point on that side
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // top
                spawnViewportCoord = new Vector3(Random.Range(0.0f, 1.0f), 1.1f, 0.0f);
                break;
            case 1: // right
                spawnViewportCoord = new Vector3(1.1f, Random.Range(0.0f, 1.0f), 0.0f);
                break;
            case 2: // bottom
                spawnViewportCoord = new Vector3(Random.Range(0.0f, 1.0f), -0.1f, 0.0f);
                break;
            case 3: // left
                spawnViewportCoord = new Vector3(-0.1f, Random.Range(0.0f, 1.0f), 0.0f);
                break;
        }
        enemy.transform.position = Camera.main.ViewportToWorldPoint(spawnViewportCoord);
    }
}
