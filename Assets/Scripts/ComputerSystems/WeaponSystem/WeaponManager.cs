using UnityEngine;

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
        return string.Empty;
    }
}
