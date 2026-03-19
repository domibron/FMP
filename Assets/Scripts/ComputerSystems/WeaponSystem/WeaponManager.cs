using System;
using UnityEngine;

[Serializable]
public class WeaponManagerData
{
    public WeaponData[] weaponsData;
}

public class WeaponManager : MonoBehaviour, IDataReadable
{
    [SerializeField]
    WeaponBase[] allWeapons;

    int activeWeaponIndex;

    InputHandler inputHandler;

    bool lastFireInput = false;

    bool waitForRelease = false;

    bool lastSwitchInput = false;

    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    void Awake()
    {
        if (allWeapons.Length > 0)
        {
            activeWeaponIndex = 0;
        }
        else
        {
            activeWeaponIndex = -1;
        }
    }

    void Update()
    {
        if (inputHandler.GetFirePressed() && !waitForRelease)
        {
            FireWeapon();
        }
        else if (!inputHandler.GetFirePressed())
        {
            waitForRelease = false;
        }

        lastFireInput = inputHandler.GetFirePressed();

        if (inputHandler.GetSwitchPressed() && !lastSwitchInput)
        {
            SwitchWeapon();
        }

        lastSwitchInput = inputHandler.GetSwitchPressed();

    }

    void FireWeapon()
    {
        if (activeWeaponIndex <= -1) return;

        allWeapons[activeWeaponIndex].ActivateWeapon();


    }

    void SwitchWeapon()
    {
        if (activeWeaponIndex + 1 >= allWeapons.Length) activeWeaponIndex = 0;
        else activeWeaponIndex++;

        if (lastFireInput) waitForRelease = true;
    }

    public string ReadData()
    {

        WeaponManagerData data = new WeaponManagerData()
        {
            weaponsData = new WeaponData[allWeapons.Length],
        };

        int count = 0;
        
        foreach (var weapon in allWeapons)
        {
            if (string.IsNullOrEmpty(weapon.ReadData()))
            {
                data.weaponsData[count] = new WeaponData()
                {
                    WeaponType = WeaponType.Cannon,
                    WeaponAmmo = -99,
                    WeaponAmmoMax = -99,
                };
                    
                count++;
                continue;
            }

            data.weaponsData[count] = JsonUtility.FromJson<WeaponData>(weapon.ReadData());
            count++;
        }
        
        return JsonUtility.ToJson(data);
    }
}
