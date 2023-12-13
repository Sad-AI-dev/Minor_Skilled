using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayerIKanimations : MonoBehaviour
    {
        [SerializeField] private Animator anim;

        [Range(0f, 2f)]
        [SerializeField] private float distanceToGround;

        [SerializeField] private LayerMask layerMask;

        private void OnAnimatorIK(int layerIndex)
        {
            if (anim)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftIKWeight"));
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftIKWeight"));
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightIKWeight"));
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightIKWeight"));

                RaycastHit leftHit;
                if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down, out leftHit, distanceToGround + 1, layerMask))
                {
                    Vector3 footPos = leftHit.point;
                    Debug.Log(footPos);
                    footPos.y += distanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
                    anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, leftHit.normal));
                }

                RaycastHit rightHit;
                if (Physics.Raycast(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down, out rightHit, distanceToGround + 1, layerMask))
                {
                    Vector3 footPos = rightHit.point;
                    Debug.Log(footPos);
                    footPos.y += distanceToGround;
                    anim.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
                    anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, rightHit.normal));
                }
            }
        }
    }
}
