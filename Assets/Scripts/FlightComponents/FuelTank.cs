using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct FuelTankData
{
    public FuelCellData[] FuelCells;


    public float FuelFillNormalized
    {
        get
        {
            float amount = 0;

            if (GetMaxTanks > 0)
            {
                foreach (var cell in FuelCells)
                {
                    amount += cell.NormalizedAmount;
                }

                amount /= GetMaxTanks;
            }

            return amount;
        }
    }

    public int GetRemainingTanks
    {
        get
        {
            int count = 0;

            foreach (var cell in FuelCells)
            {
                if (cell.MaxAmount == -1) continue;
                count++;
            }

            return count;
        }
    }

    public int GetMaxTanks
    {
        get { return FuelCells.Length; }
    }
}

public class FuelTank : MonoBehaviour, IConsumable, IDataReadable
{
    [SerializeField]
    FuelCell[] fuelCells;

    [SerializeField]
    bool infFuel = false;

    public bool CanConsume()
    {
        if (infFuel) return true;

        List<FuelCell> availableFuelCells = new List<FuelCell>();

        foreach (var fuelCell in fuelCells)
        {
            if (JsonUtility.FromJson<FuelCellData>(fuelCell.ReadData()).Amount > 0)
            {
                availableFuelCells.Add(fuelCell);
            }
        }

        if (availableFuelCells.Count <= 0) return false; // cannot consume any fuel form any cells.

        return true;
    }

    public bool Consume(float amount)
    {
        if (infFuel) return true;

        List<FuelCell> availableFuelCells = new List<FuelCell>();

        foreach (var fuelCell in fuelCells)
        {
            if (JsonUtility.FromJson<FuelCellData>(fuelCell.ReadData()).Amount > 0)
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
        FuelTankData fuelTankData = new FuelTankData
        {
            FuelCells = new FuelCellData[fuelCells.Length]
        };

        int count = 0;

        foreach (var fuelCell in fuelCells)
        {
            fuelTankData.FuelCells[count] = JsonUtility.FromJson<FuelCellData>(fuelCell.ReadData());
            count++;
        }

        return JsonUtility.ToJson(fuelTankData);

        // List<FuelCellData> fuelCellsData = new List<FuelCellData>();

        // foreach (var fuelCell in fuelCells)
        // {
        //     fuelCellsData.Add(JsonUtility.FromJson<FuelCellData>(fuelCell.ReadData()));
        // }

        // fuelTankData.FuelCells = fuelCellsData.ToArray();

        // return JsonUtility.ToJson(new FuelTankData
        // {
        //     FuelCells = fuelCellsData.ToArray()
        // });
    }
}
