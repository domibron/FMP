using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ComponentBase), true)]
public class ComponentBaseDestory : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ComponentBase cBase = (ComponentBase)target;

        if (GUILayout.Button("Destroy"))
        {
            if (Application.isPlaying)
                cBase.DealDamage(999999);
        }

        if (GUILayout.Button("Reset"))
        {
            if (Application.isPlaying)
                cBase.ResetComponent();
        }
    }
}
#endif

public class ComponentBase : MonoBehaviour
{
    [SerializeField, Header("Component")]
    protected float maxHealth = 100f;

    [SerializeField]
    protected string compName = "Name";

    public string ComponentName { get { return compName; } }
    public float Health { get { return currentHealth; } }
    public float HealthNormalized { get { return currentHealth / maxHealth; } }
    public float MaxHealth { get { return maxHealth; } }

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
            currentHealth = 0;
        }
    }

    public bool IsComponentDestroyed()
    {
        return destroyed;
    }
}
