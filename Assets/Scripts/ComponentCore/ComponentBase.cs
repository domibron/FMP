using UnityEngine;

public class ComponentBase : MonoBehaviour
{
    [SerializeField, Header("Component")]
    protected float maxHealth = 100f;

    protected float currentHealth;

    protected bool destroyed = false;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        destroyed = false;
    }

    protected virtual void DestroyComponent()
    {
        destroyed = true;
    }

    public virtual void ResetComponent()
    {
        currentHealth = maxHealth;
        destroyed = false;
    }

    public void DealDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            DestroyComponent();
        }
    }
}
