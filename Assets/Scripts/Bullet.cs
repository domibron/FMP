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

    Vector3 lastPos;
    // bool gotLastPos

    bool canCollide = false;

    float waitTime = 0f;

    float waitDuration = 0.3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        // if (Physics.Linecast(transform.position, lastPos, layerMask))
        // {
        //     print("Shit, something was hit");
        // }

        if (waitTime > waitDuration) canCollide = true;
        else waitTime += Time.fixedDeltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rb.linearVelocity.magnitude, layerMask))
        {
            // print("Shit, something was hit^2");

            if (hit.collider.gameObject.CompareTag(Constants.SHIP_TAG) || hit.collider.gameObject.CompareTag(Constants.MISSILE_TAG))
            {
                RaycastHit[] hits = Physics.RaycastAll(hit.point, GetComponent<Rigidbody>().linearVelocity.normalized, depth, layerMask, QueryTriggerInteraction.Ignore);

                Debug.DrawRay(hit.point, rb.linearVelocity.normalized * depth, Color.blue, 60f);

                if (hit.transform.GetComponent<ComponentBase>())
                {
                    hit.transform.GetComponent<ComponentBase>().DealDamage(damage);
                    print(hit.collider.gameObject.name + " was hit first");
                }

                if (hits.Length <= 0) return;

                foreach (var h in hits)
                {
                    h.collider.GetComponent<ComponentBase>()?.DealDamage(damage);
                    print(h.collider.name + " was hit");
                }

                Destroy(gameObject);
            }
            else
            {

                rb.linearVelocity = Vector3.Reflect(rb.linearVelocity * 0.8f, hit.normal);

                if (rb.linearVelocity.magnitude <= Cannon.BULLET_SPEED * 0.5f)
                {
                    Destroy(gameObject);
                }

                transform.position = hit.point + (rb.linearVelocity.normalized * 2f);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!canCollide) return; // ignore

        if (collision.gameObject.CompareTag(Constants.SHIP_TAG) || collision.gameObject.CompareTag(Constants.MISSILE_TAG))
        {
            RaycastHit[] hits = Physics.RaycastAll(collision.GetContact(0).point, GetComponent<Rigidbody>().linearVelocity.normalized, depth, layerMask, QueryTriggerInteraction.Ignore);

            Debug.DrawRay(collision.GetContact(0).point, rb.linearVelocity.normalized * depth, Color.blue, 60f);

            if (collision.transform.GetComponent<ComponentBase>())
            {
                collision.transform.GetComponent<ComponentBase>().DealDamage(damage);
                print(collision.gameObject.name + " was hit first");
            }

            if (hits.Length <= 0) return;

            foreach (var hit in hits)
            {
                hit.collider.GetComponent<ComponentBase>()?.DealDamage(damage);
                print(hit.collider.name + " was hit");
            }



            // if (collision.rigidbody)
            // {

            //     AddForceToBody(collision.rigidbody);
            //     print("Has rb");
            // }
            // else if (collision.transform.GetComponentInParent<Rigidbody>() != null)
            // {
            //     Rigidbody hitRb = collision.transform.GetComponentInParent<Rigidbody>();

            //     AddForceToBody(hitRb);
            //     print("Has rb 2");
            // }

            // void AddForceToBody(Rigidbody hitRB)
            // {
            //     float mag = collision.relativeVelocity.magnitude;

            //     Debug.DrawLine(collision.GetContact(0).point, collision.GetContact(0).point + collision.relativeVelocity, Color.red, 60f);
            //     hitRB.AddForceAtPosition(collision.relativeVelocity.normalized * (mag * (rb.mass / hitRB.mass)), collision.GetContact(0).point, ForceMode.Impulse);
            // }

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
