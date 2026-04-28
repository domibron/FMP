using System;
using UnityEngine;

[Serializable]
public struct FuelCellData
{
    public int ID;
    public float MaxAmount;
    public float Amount;

    public float NormalizedAmount
    {
        get
        {
            return Amount / MaxAmount;
        }
    }
}

public class FuelCell : ComponentBase, IConsumable, IDataReadable
{
    [Space, Header("Fuel Cell")]
    [SerializeField]
    int id = 0;

    [SerializeField]
    float maxFuel = 100f;


    float currentFuel;

    protected override void Awake()
    {
        base.Awake();

        currentFuel = maxFuel;
    }

    public override void ResetComponent()
    {
        base.ResetComponent();

        currentFuel = maxFuel;
    }


    public bool Consume(float amount)
    {
        if (currentFuel <= 0) return false;

        currentFuel -= amount; // aware of negative fuel, will fix later.

        return true;
    }

    protected override void DestroyComponent()
    {
        base.DestroyComponent();

        currentFuel = 0;
    }

    public string ReadData()
    {
        return JsonUtility.ToJson(new FuelCellData
        {
            ID = id,
            MaxAmount = maxFuel,
            Amount = currentFuel,
        });
    }
}
