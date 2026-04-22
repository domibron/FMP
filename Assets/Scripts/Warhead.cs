using System;
using UnityEngine;

public class Warhead : ComponentBase
{
    [SerializeField]
    float radius = 5f;

    private bool isArmed = false;

    [SerializeField]
    float damage = 999f;

    [SerializeField]
    LayerMask layerMask;

    private bool detonated = false;

    public event Action OnDetonation;

    [SerializeField]
    GameObject particlesPrefab;

    public void Arm()
    {
        isArmed = true;
    }

    public void Disarm()
    {
        isArmed = false;
    }

    public bool Detonate()
    {
        if (!isArmed || detonated) return false;
        detonated = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask, QueryTriggerInteraction.Ignore);

        foreach (var col in colliders)
        {
            ComponentBase cBase = col.GetComponent<ComponentBase>();

            if (cBase != null)
            {
                cBase.DealDamage(damage);
            }

            IMissileHit missileHit = col.GetComponent<IMissileHit>();

            if (missileHit != null) missileHit.Hit(transform.position);
        }

        if (particlesPrefab != null)
            Instantiate(particlesPrefab, transform.position, transform.rotation);

        OnDetonation.Invoke();

        return true;
    }

    protected override void DestroyComponent()
    {
        base.DestroyComponent();

        Detonate();
    }
}
