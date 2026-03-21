using UnityEngine;

public class MissileWeaponController : WeaponBase
{
    [SerializeField]
    MissileMount[] missileMounts;

    float currentTimeOut = 0;

    float waitTime = 0.05f;

    void Update()
    {
        if (currentTimeOut > 0) currentTimeOut -= Time.deltaTime;
    }

    public override void ActivateWeapon()
    {
        if (currentTimeOut <= 0)
        {
            FireWeapon();
        }

        currentTimeOut = waitTime;
    }

    public override void FireWeapon()
    {
        foreach (MissileMount missileMount in missileMounts)
        {
            if (string.IsNullOrEmpty(missileMount.ReadData())) continue;
            if (JsonUtility.FromJson<MissileMountData>(missileMount.ReadData()).hasMissile)
            {
                missileMount.Activate();
                return;
            }

        }

        // Rearm(); // ! Remove
    }

    public override void Rearm()
    {
        foreach (var missileMount in missileMounts)
        {
            missileMount.Rearm();
        }
    }

    public override string ReadData()
    {
        int count = 0;
        foreach (var missileMount in missileMounts)
        {
            if (string.IsNullOrEmpty(missileMount.ReadData())) continue;

            if (!JsonUtility.FromJson<MissileMountData>(missileMount.ReadData()).hasMissile) continue;

            count++;
        }

        return JsonUtility.ToJson(new WeaponData()
        {
            WeaponType = WeaponType.Missile,
            WeaponAmmo = count,
            WeaponAmmoMax = missileMounts.Length,
        });
    }
}
