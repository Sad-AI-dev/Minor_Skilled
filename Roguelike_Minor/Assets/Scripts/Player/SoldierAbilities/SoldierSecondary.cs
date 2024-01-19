using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Game.Player.Soldier
{
    [CreateAssetMenu(fileName = "SoldierSecondary", menuName = "ScriptableObjects/Agent/Ability/Soldier/Secondary")]
    public class SoldierSecondary : AbilitySO
    {

        [SerializeField] private GameObject grenade;
        [SerializeField] private LayerMask layermask;
        [SerializeField] private float projectileVelocity;
        [SerializeField] private float upwardVelocity;
        [SerializeField] private float projectileGravity;

        private PlayerController controller;
        private GrenadeProjectile grenadeProjectile;


        [Header("Poison")]
        [SerializeField] int defaultPoisonOrbs = 4;
        [SerializeField] int upgradePoisonOrbs = 1;
        [SerializeField] private float poisonDamageMultiplier;

        public class SecondaryVars : Ability.AbilityVars
        {
            public int totalPoisonOrbs;
        }


        public override void InitializeVars(Ability source)
        {
            source.vars = new SecondaryVars { totalPoisonOrbs = defaultPoisonOrbs };
            controller = source.agent.GetComponent<PlayerController>();
        }

        public override void Use(Ability source)
        {
            Camera cam = Camera.main;
            Vector3 target;
            Vector3 bulletDir;

            controller.StartSlowCoroutine(.2f);

            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportPointToRay(new UnityEngine.Vector3(0.5f, 0.5f, 0)), out hit, 500, layermask))
                target = hit.point;
            else
                target = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 500));

            bulletDir = (target - source.originPoint.position).normalized;

            GameObject projectile = Instantiate(grenade, source.originPoint.position, Quaternion.identity);
            projectile.transform.LookAt(target);
            grenadeProjectile = projectile.GetComponent<GrenadeProjectile>();
            grenadeProjectile.velocity = bulletDir * projectileVelocity;
            grenadeProjectile.Initialize(source);
            grenadeProjectile.gravity = projectileGravity;
            grenadeProjectile.upwardVelocity = upwardVelocity;
            grenadeProjectile.poisonGrenadeAmount = (source.vars as SecondaryVars).totalPoisonOrbs;
            grenadeProjectile.explosionDamage = source.abilityData.damageMultiplier * source.agent.stats.baseDamage;
            grenadeProjectile.poisonGrenadeDamage = source.agent.stats.baseDamage * poisonDamageMultiplier;
        }

        public override void Upgrade(Ability source)
        {

            (source.vars as SecondaryVars).totalPoisonOrbs += upgradePoisonOrbs;
        }

        public override void DownGrade(Ability source)
        {
            SecondaryVars vars = source.vars as SecondaryVars;
            if(vars.totalPoisonOrbs > defaultPoisonOrbs)
                vars.totalPoisonOrbs -= upgradePoisonOrbs;
        }
    }
}
