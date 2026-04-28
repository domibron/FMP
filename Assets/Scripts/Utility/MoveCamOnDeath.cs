using System;
using UnityEngine;

public class MoveCamOnDeath : MonoBehaviour
{
    private Vector3 curPos;

    private GameManager gameManager;

    private Vector3 velocity;

    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.OnPlayerDeath += OnPlayerDeath;
    }

    void Update()
    {
        transform.LookAt(curPos);

        curPos += velocity * Time.deltaTime;
    }

    private void OnPlayerDeath(Vector3 pos, Vector3 vel)
    {
        transform.position = pos;

        transform.position += UnityEngine.Random.insideUnitSphere * 30f;

        curPos = pos;

        velocity = vel;
    }
}
