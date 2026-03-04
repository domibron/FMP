using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField]
    float destroyAfter = 5f;

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}
