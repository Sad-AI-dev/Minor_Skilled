using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PoisonGrenade : Projectile
    {
        [SerializeField] private GameObject poisonCloud;
        [SerializeField] private AudioPlayer AP;

        [HideInInspector] public float gravity;
        
        

        protected override void UpdateMoveDir()
        {
            Debug.Log(velocity);
            velocity += new Vector3(0, -gravity, 0);
        }

        protected override void CustomCollide(Collider other)
        {
            GameObject poison = Instantiate(poisonCloud, transform.position, Quaternion.identity);
            poison.GetComponent<DOTExplosion>().source = source;
            AP.Play(0);
        }
    }
}
