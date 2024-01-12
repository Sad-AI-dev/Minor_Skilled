using UnityEngine;
using UnityEngine.AI;
using Game.Core;
using Game.Core.Data;

namespace Game.Enemy {
    [CreateAssetMenu(fileName = "MeleeGruntSecondary", menuName = "ScriptableObjects/Enemy/MeleeGrunt/Secondary")]
    public class MeleeGruntSecondarySO : AbilitySO
    {
        public GameObject prefab;
        public float bulletSpeed;
        public class MeleeGruntSecondaryVars : Ability.AbilityVars
        {
            public BehaviourPool<MeleeGruntSecondaryBehaviour> behaviourPool;
            public Transform target;
            public Transform transform;
            public NavMeshAgent navAgent;
        }

        public override void InitializeVars(Ability source)
        {
            BehaviourPool<MeleeGruntSecondaryBehaviour> pool = new BehaviourPool<MeleeGruntSecondaryBehaviour>();
            pool.behaviourTemplate = prefab;
            source.vars = new MeleeGruntSecondaryVars
            {
                behaviourPool = pool,
                transform = source.agent.transform,
                navAgent = source.agent.GetComponent<NavMeshAgent>()
            };
        }

        public override void Use(Ability source)
        {
            MeleeGruntSecondaryVars vars = (source.vars as MeleeGruntSecondaryVars);
            MeleeGruntSecondaryBehaviour secondary = vars.behaviourPool.GetBehaviour();
            if (null == vars.target) vars.target = (Transform)source.agent.GetComponent<Core.Tree>().root.GetData("Target");
            vars.navAgent.velocity = Vector3.zero;

            Vector3 targetPostition = new Vector3(vars.target.position.x, vars.transform.position.y, vars.target.position.z);
            vars.transform.LookAt(targetPostition);

            secondary.gameObject.transform.position = source.originPoint.position;
            secondary.target = GameStateManager.instance.player.transform.position;
            secondary.sourceTransform = source.originPoint.position;
            secondary.bulletSpeed = bulletSpeed;
            secondary.sampleTime = 0;
            secondary.Initialize(source);
        }
    }
}
