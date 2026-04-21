using UnityEngine;

public class ShipExpload : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    float currentTimer = 0f;

    bool secondTriggered = false;
    bool thirdTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;

        if (!secondTriggered && currentTimer >= 0.3f)
        {
            audioSource.Play();
            secondTriggered = true;
        }

        if (!thirdTriggered && currentTimer >= 0.7f)
        {
            audioSource.Play();
            thirdTriggered = true;
        }
    }
}
