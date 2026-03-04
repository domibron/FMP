using UnityEngine;

public enum WeaponType
{
    Missile,
    Cannon,
}

public class WeaponData
{
    public WeaponType WeaponType;
    public int WeaponAmmo;
    public int WeaponAmmoMax; // ? might not need this.
}

public abstract class WeaponBase : MonoBehaviour, IDataReadable
{
    public virtual string ReadData()
    {
        return string.Empty;
    }

    public abstract void ActivateWeapon();

    public abstract void FireWeapon();

    public abstract void Rearm();
}
