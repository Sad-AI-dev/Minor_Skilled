using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class SoldierAbilities : MonoBehaviour
    {
        private Agent agent;
        [SerializeField] private SoldierPrimarySO soldierPrimary;

        private void Start()
        {
            agent = GetComponent<Agent>();
            agent.abilities.primary.onUse.AddListener(Primary);
        }

        private void OnDisable()
        {
            agent.abilities.primary.onUse.RemoveListener(Primary);
        }

        private void Primary()
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 500))
            {
                target = hit.point;
                //Instantiate(marker, hit.point, Quaternion.identity);
            }
            else
            {
                target = cam.transform.forward * 10000;
            }

            GameObject projectile = Instantiate(soldierPrimary.bullet, agent.abilities.primary.originPoint.position, Quaternion.identity);

            bulletDir = (target - agent.abilities.primary.originPoint.position).normalized;
            projectile.GetComponent<PlayerBullet>().moveDir = bulletDir * soldierPrimary.bulletSpeed;
        }
    }
}
