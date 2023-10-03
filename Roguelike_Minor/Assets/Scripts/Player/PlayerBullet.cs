using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerBullet : MonoBehaviour
    {
        [HideInInspector] public Vector3 moveDir;

        private void FixedUpdate()
        {
            transform.position += moveDir;
        }

        private void OnTriggerEnter(Collider other)
        {
            //damage target / destroy bullet
        }
    }
}
