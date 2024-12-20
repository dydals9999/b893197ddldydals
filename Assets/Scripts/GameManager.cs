using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public GameObject gameOverSet;

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay) {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }
        // ui score update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }
    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyObjs[ranEnemy],
                           spawnPoints[ranPoint].position,
                           spawnPoints[ranPoint].rotation);
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if(ranPoint == 5 || ranPoint == 7) { //Right spawn
            enemy.transform.Rotate(Vector3.back * 37);
            rigid.velocity = new Vector2(enemyLogic.speed *(-1), -1);
        }
        else if (ranPoint == 6 || ranPoint == 8) { //Left Spawn
            enemy.transform.Rotate(Vector3.forward * 37);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else {  //Front spawn
            rigid.velocity = new Vector2(0, enemyLogic.speed*(-1));
        }
    }
    public void UpdateLifeIcon(int life)
    {
        // ui init disable 
        for (int index=0; index < 3; index++) {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
        // ui init Ac tive
        for (int index = 0; index <life ; index++) {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }
    public void RespawnPlayer() 
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 4.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false; 
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
