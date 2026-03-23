using System;
using UnityEngine;

[Serializable]
public class CountermeasureDispenserData
{
    public int maxCount;
    public int currentCount;
    public float maxCooldown;
    public float currentCooldownTime;
}

public class CountermeasureDispenser : ComponentBase, IActivateable, IDataReadable
{
    [SerializeField, Header("Countermeasure Dispenser")]
    int maxCount = 20;

    int currentCount = 0;

    [SerializeField]
    GameObject smokePrefab;

    [SerializeField]
    float cooldown = 0.2f;

    private float currentCooldownTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        currentCount = maxCount;
    }

    private void Update()
    {
        if (currentCooldownTime > 0) currentCooldownTime -= Time.deltaTime;
    }

    private void Dispense()
    {
        if (currentCount <= 0 || currentCooldownTime > 0) return;

        currentCount -= 1;

        Instantiate(smokePrefab, transform.position, transform.rotation);

        currentCooldownTime = cooldown;
    }

    public void Rearm()
    {
        currentCount = maxCount;
    }

    public void Activate()
    {
        Dispense();
    }

    public string ReadData()
    {
        if (destroyed) return string.Empty;

        CountermeasureDispenserData data = new CountermeasureDispenserData()
        {
            maxCount = maxCount,
            currentCount = currentCount,
            maxCooldown = cooldown,
            currentCooldownTime = currentCooldownTime,
        };

        return JsonUtility.ToJson(data);
    }
}
