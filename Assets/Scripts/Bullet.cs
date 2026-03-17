using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float depth = 3f;

    [SerializeField]
    float damage = 100f;

    [SerializeField]
    LayerMask layerMask;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.SHIP_TAG))
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, GetComponent<Rigidbody>().linearVelocity.normalized, depth, layerMask, QueryTriggerInteraction.Ignore);

            Debug.DrawRay(transform.position, rb.linearVelocity.normalized * depth, Color.blue, 60f);

            if (hits.Length <= 0) return;

            foreach (var hit in hits)
            {
                hit.collider.GetComponent<ComponentBase>()?.DealDamage(damage);
                print(hit.collider.name + " was hit");
            }

            Destroy(gameObject);
        }
        else
        {

            rb.linearVelocity = Vector3.Reflect(rb.linearVelocity * 0.8f, collision.GetContact(0).normal);

            if (rb.linearVelocity.magnitude <= Cannon.BULLET_SPEED * 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}
