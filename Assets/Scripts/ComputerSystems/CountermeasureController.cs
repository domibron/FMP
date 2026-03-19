using System;
using UnityEngine;

[Serializable]
public class CountermeasureControllerData
{
    public CountermeasureDispenserData[] dispensersData;    
}

public class CountermeasureController : MonoBehaviour, IDataReadable
{
    [SerializeField]
    CountermeasureDispenser[] dispensers;

    InputHandler inputHandler;

    private ComponentBase componentBase;
    
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        
        componentBase =  GetComponent<ComponentBase>();
    }

    private void Update()
    {
        if (inputHandler.GetCounterMPressed())
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
                data.dispensersData[count] = new  CountermeasureDispenserData() // failure data, data that is impossible to be at.
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
