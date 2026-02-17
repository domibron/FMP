using System.Collections.Generic;
using UnityEngine;

struct FuelTankData
{
    public FuelCellData[] fuelCells;
}

public class FuelTank : MonoBehaviour, IConsumable, IDataReadable
{
    [SerializeField]
    FuelCell[] fuelCells;

    public bool Consume(float amount)
    {
        List<FuelCell> availableFuelCells = new List<FuelCell>();

        foreach (var fuelCell in fuelCells)
        {
            if (JsonUtility.FromJson<FuelCellData>(fuelCell.ReadData()).amount > 0)
            {
                availableFuelCells.Add(fuelCell);
            }
        }

        if (availableFuelCells.Count <= 0) return false; // cannot consume any fuel form any cells.

        float amountToTakePerTank = amount / availableFuelCells.Count;

        foreach (var fuelCell in availableFuelCells)
        {
            fuelCell.Consume(amountToTakePerTank);
        }

        return true;
    }

    public string ReadData()
    {
        List<FuelCellData> fuelCellsData = new List<FuelCellData>();

        foreach (var fuelCell in fuelCells)
        {
            fuelCellsData.Add(JsonUtility.FromJson<FuelCellData>(fuelCell.ReadData()));
        }

        return JsonUtility.ToJson(new FuelTankData
        {
            fuelCells = fuelCellsData.ToArray()
        });
    }
}
