using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    Team.TeamType teamType;

    [SerializeField]
    float range = 250f;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform bulletSpawnPoint;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    Transform turretHead;

    List<Rigidbody> allTargets = new List<Rigidbody>();

    float bulletSpeed = 800f;


    void FixedUpdate()
    {
        Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, range, layerMask);

        if (detectedEnemies.Length <= 0) return;

        allTargets.Clear();

        foreach (Collider detectedEnemy in detectedEnemies)
        {
            if (detectedEnemy.gameObject.CompareTag(Constants.SHIP_TAG))
            {
                // if (detectedEnemy.transform.root.GetComponent<Team>().GetTeamType == teamType) continue;

                if (detectedEnemy.GetComponent<Team>() == null) continue;

                if (detectedEnemy.GetComponent<Team>().GetTeamType == teamType) continue;

                allTargets.Add(detectedEnemy.GetComponentInParent<Rigidbody>());
            }
            else if (detectedEnemy.gameObject.CompareTag(Constants.MISSILE_TAG))
            {
                if (detectedEnemy.transform.root.CompareTag(Constants.SHIP_TAG))
                {
                    if (detectedEnemy.transform.root.GetComponent<Team>())
                    {
                        if (detectedEnemy.transform.root.GetComponent<Team>().GetTeamType == teamType) continue;
                    }
                }

                allTargets.Add(detectedEnemy.GetComponentInParent<Rigidbody>());
            }
        }

        if (allTargets.Count <= 0) return;

        allTargets.Sort((a, b) =>
        {
            float aDist = Vector3.Distance(a.transform.position, transform.position);
            float bDist = Vector3.Distance(b.transform.position, transform.position);

            if (aDist > bDist) return 1;
            else if (aDist < bDist) return -1;
            return 0;
        });

        float disFromBar = Vector3.Distance(bulletSpawnPoint.position, transform.position);

        Vector3 targetPos = allTargets[0].transform.position;

        Vector3 spawn = transform.position + ((allTargets[0].transform.position - transform.position).normalized * disFromBar);

        float dist = Vector3.Distance(targetPos, spawn);

        turretHead.LookAt(targetPos + (allTargets[0].linearVelocity * Time.fixedDeltaTime) + (allTargets[0].linearVelocity * (dist / bulletSpeed)));

        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.VelocityChange);
    }
}
