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

    private GameObject player;

    private int enemyKills;
    private int playerKills;

    private int enemyDeaths;
    private int playerDeaths;

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
            Destroy(player);

            CreatePlayer();

            playerDeaths++;
            enemyKills++;
        }
        else if (teamType == Team.TeamType.TeamB)
        {
            Destroy(enemy);

            CreateEnemy();

            playerKills++;
            enemyDeaths++;
        }
    }

    private void CreatePlayer()
    {
        player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);

    }

    private void CreateEnemy()
    {
        enemy = Instantiate(enemyPrefab, enemySpawn.position, enemySpawn.rotation);
    }
}
