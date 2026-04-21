using UnityEngine;

public class BlowUp : MonoBehaviour
{
    [SerializeField]
    FuelTank fuelTank;

    [SerializeField]
    WeaponManager weaponManager;

    [SerializeField]
    Team.TeamType teamType = Team.TeamType.TeamB;

    void Update()
    {
        if (JsonUtility.FromJson<WeaponManagerData>(weaponManager.ReadData()).WeaponsData[1].WeaponAmmoNormalized <= 0)
        {
            GameManager.Instance.SelfDestruct(teamType);
        }

        if (JsonUtility.FromJson<FuelTankData>(fuelTank.ReadData()).FuelFillNormalized <= 0)
        {
            GameManager.Instance.SelfDestruct(teamType);
        }
    }
}
