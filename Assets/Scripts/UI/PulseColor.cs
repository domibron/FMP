using UnityEngine;
using UnityEngine.UI;

public class PulseColor : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField, Range(0, 1)]
    float minAlpha = 0;
    [SerializeField, Range(0, 1)]
    float maxAlpha = 1;

    [SerializeField]
    float flashRate = 1f;

    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime * flashRate;

        Color color = image.color;

        color.a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.Pow(Mathf.Sin(timer * Mathf.PI), 2f));

        image.color = color;

    }
}
