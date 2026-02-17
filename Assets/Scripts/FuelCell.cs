using UnityEngine;

public struct FuelCellData
{
    public int id;
    public float maxAmount;
    public float amount;
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

    public bool Consume(float amount)
    {
        if (currentFuel <= 0) return false;

        currentFuel -= amount; // aware of negative fuel, will fix later.

        return true;
    }

    public string ReadData()
    {
        return JsonUtility.ToJson(new FuelCellData
        {
            id = id,
            maxAmount = maxFuel,
            amount = currentFuel,
        });
    }
}
