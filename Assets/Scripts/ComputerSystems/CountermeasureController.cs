using System;
using UnityEngine;

[Serializable]
public class CountermeasureControllerData
{
    public CountermeasureDispenserData[] dispensersData;

    public int GetRemaining
    {
        get
        {
            int count = 0;
            float avg = 0;

            foreach (CountermeasureDispenserData dispenser in dispensersData)
            {
                if (dispenser.maxCount == -1) continue;

                avg += (float)dispenser.currentCount;

                count++;
            }

            return Mathf.RoundToInt(avg / count);
        }
    }

    public int GetMax
    {
        get
        {
            int count = 0;
            float avg = 0;

            foreach (CountermeasureDispenserData dispenser in dispensersData)
            {
                if (dispenser.maxCount == -1) continue;

                avg += dispenser.maxCount;

                count++;
            }

            return Mathf.RoundToInt(avg / count);
        }
    }

    public float GetRemainingNormalized
    {
        get
        {
            return (float)GetRemaining / GetMax;
        }
    }
}

public class CountermeasureController : MonoBehaviour, IDataReadable
{
    [SerializeField]
    CountermeasureDispenser[] dispensers;

    InputIntermediate inputIntermediate;

    private ComponentBase componentBase;

    private void Awake()
    {
        inputIntermediate = GetComponent<InputIntermediate>();

        componentBase = GetComponent<ComponentBase>();
    }

    private void Update()
    {
        if (inputIntermediate.GetCounterMPressed())
        {
            DispenseAll();
        }
    }

    private void DispenseAll()
    {
        foreach (CountermeasureDispenser dispenser in dispensers)
        {
            dispenser.Activate();
        }
    }

    public void RearmAll()
    {
        foreach (CountermeasureDispenser dispenser in dispensers)
        {
            dispenser.Rearm();
        }
    }


    public string ReadData()
    {
        if (componentBase.IsComponentDestroyed()) return string.Empty;

        CountermeasureControllerData data = new CountermeasureControllerData();
        data.dispensersData = new CountermeasureDispenserData[dispensers.Length];

        int count = 0;

        foreach (var dispenser in dispensers)
        {
            if (string.IsNullOrEmpty(dispenser.ReadData()))
            {
                data.dispensersData[count] = new CountermeasureDispenserData() // failure data, data that is impossible to be at.
                {
                    currentCount = -1,
                    currentCooldownTime = -1,
                    maxCount = -1,
                    maxCooldown = -1,
                };
            }
            else
            {
                data.dispensersData[count] = JsonUtility.FromJson<CountermeasureDispenserData>(dispenser.ReadData());
            }

            count++;
        }

        return JsonUtility.ToJson(data);
    }
}
