using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public enum frictionTypes
    {
        air,
        ground,
        dash,
        jump
    }

    public class FrictionManager : MonoBehaviour
    {
        [Header("Friction")]
        [SerializeField] private float airFriction;
        [SerializeField] private float groundFriction;
        [SerializeField] private float dashFriction;
        [SerializeField] private float jumpFriction;
        private float activeFriction;

        public void SetFriction(frictionTypes typeEnum)
        {
            switch(typeEnum)
            {
                case frictionTypes.air:
                    activeFriction = airFriction;
                    break;
                case frictionTypes.ground: 
                    activeFriction = groundFriction;
                    break;
                case frictionTypes.dash: 
                    activeFriction = dashFriction;
                    break;
                case frictionTypes.jump:
                    activeFriction = jumpFriction;
                    break;
            }
        }

        public Vector3 ApplyFriction(Vector3 velocity)
        {
            //Debug.Log(activeFriction);
            float drag = 0.5f * activeFriction * velocity.magnitude * 0.47f * 2;
            velocity -= velocity.normalized * drag * Time.fixedDeltaTime;
            return velocity;
        }
    }
}
