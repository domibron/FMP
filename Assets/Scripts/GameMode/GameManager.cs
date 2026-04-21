using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    GameObject enemyPrefab;

    [SerializeField]
    Transform enemySpawn;

    private GameObject enemy;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    Transform playerSpawn;

    [SerializeField]
    GameObject shipRagDoll;

    private GameObject player;

    private int enemyKills;
    private int playerKills;

    private int enemyDeaths;
    private int playerDeaths;

    private Coroutine playerRespawn;
    private Coroutine enemyRespawn;

    public event Action<Vector3, Vector3> OnPlayerDeath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        CreateEnemy();
        CreatePlayer();
    }

    public void SelfDestruct(Team.TeamType teamType)
    {
        if (teamType == Team.TeamType.TeamA)
        {
            Rigidbody shipRB = player.GetComponent<Rigidbody>();

            SpawnRagDoll(player);

            OnPlayerDeath?.Invoke(player.transform.position, shipRB.linearVelocity);

            Destroy(player);

            playerRespawn = StartCoroutine(PlayerRespawn());
            playerDeaths++;
            enemyKills++;
        }
        else if (teamType == Team.TeamType.TeamB)
        {
            Rigidbody shipRB = enemy.GetComponent<Rigidbody>();

            SpawnRagDoll(enemy);

            Destroy(enemy);

            enemyRespawn = StartCoroutine(EnemyRespawn());

            playerKills++;
            enemyDeaths++;
        }
    }

    private void SpawnRagDoll(GameObject ship)
    {
        Rigidbody shipRB = ship.GetComponent<Rigidbody>();

        GameObject ragDoll = Instantiate(shipRagDoll, ship.transform.position, ship.transform.rotation);

        Rigidbody ragRigid = ragDoll.GetComponent<Rigidbody>();

        ragRigid.linearVelocity = shipRB.linearVelocity;
        ragRigid.angularVelocity = shipRB.angularVelocity;

        Destroy(ragDoll, 5f);
    }

    private void CreatePlayer()
    {
        player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);

    }

    private void CreateEnemy()
    {
        enemy = Instantiate(enemyPrefab, enemySpawn.position, enemySpawn.rotation);
    }

    private IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(3f);

        CreatePlayer();
        playerRespawn = null;
    }

    private IEnumerator EnemyRespawn()
    {
        yield return new WaitForSeconds(3f);

        CreateEnemy();
        enemyRespawn = null;
    }

    public bool IsPlayerDead()
    {
        return !player;
    }
}
