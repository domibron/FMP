using UnityEngine;

public class CamToggleForPlayer : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField]
    Camera cam;

    [SerializeField]
    AudioListener audioListener;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        cam.enabled = gameManager.IsPlayerDead();
        audioListener.enabled = gameManager.IsPlayerDead();
    }
}
