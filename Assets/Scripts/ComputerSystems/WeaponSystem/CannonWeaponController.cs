using UnityEngine;

public class CannonWeaponController : WeaponBase
{
    [SerializeField]
    Cannon cannon;

    private bool isFiringCannon = false;

    private float inputTimeOut = 0;
    private float inputDetectionWait = 0.05f;


    void Update()
    {
        if (inputTimeOut > 0)
        {
            isFiringCannon = true;
            inputTimeOut -= Time.deltaTime;
        }
        else
        {
            isFiringCannon = false;
        }

        FireWeapon();
    }

    public override string ReadData()
    {
        CannonData cannonData = new CannonData();

        if (!string.IsNullOrEmpty(cannon.ReadData()))
            cannonData = JsonUtility.FromJson<CannonData>(cannon.ReadData());

        return JsonUtility.ToJson(new WeaponData()
        {
            WeaponType = WeaponType.Cannon,
            WeaponAmmo = cannonData.CurrentAmmo,
            WeaponAmmoMax = cannonData.MaxAmmo,
        });
    }

    public override void ActivateWeapon()
    {
        inputTimeOut = inputDetectionWait;
    }

    public override void FireWeapon()
    {
        cannon.SetFireState(isFiringCannon);
    }

    public override void Rearm()
    {
        cannon.Rearm();
    }
}
