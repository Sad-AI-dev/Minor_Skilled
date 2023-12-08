using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    public class Boss_BigSquid_SecondaryVars : Ability.AbilityVars
    {
        public Transform target;
    }

    [CreateAssetMenu(fileName = "Boss_BigSquid_Secondary", menuName = "ScriptableObjects/Enemy/Boss/BigSquid/SecondaryAttack")]
    public class Boss_BigSquid_SecondarySO : AbilitySO
    {
        public GameObject bulletPrefab;
        public float bulletSpeed = 5;

        public override void InitializeVars(Ability source)
        {

        }

        public override void Use(Ability source)
        {
            Boss_BigSquid_SecondaryVars vars = source.vars as Boss_BigSquid_SecondaryVars;

            source.agent.StartCoroutine(ShootBulletsCo(source, vars.target));
        }

        IEnumerator ShootBulletsCo(Ability source, Transform target)
        {
            List<GameObject> bullets = new List<GameObject>();

            for (int i = 0; i < source.originPoint.childCount; i++)
            {
                bullets.Add(Instantiate(bulletPrefab, source.originPoint.GetChild(i)));
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2);

            //Shoot in general direction
            for (int i = 0; i < bullets.Count; i++)
            {
                Boss_BigSquid_SecondaryBehaviour behaviour = bullets[i].GetComponent<Boss_BigSquid_SecondaryBehaviour>();
                bullets[i].transform.parent = null;
                behaviour.bulletSpeed = bulletSpeed;
                behaviour.source = source;
                source.agent.StartCoroutine(behaviour.RandomFireCo(target));
            }
        }
    }
}
