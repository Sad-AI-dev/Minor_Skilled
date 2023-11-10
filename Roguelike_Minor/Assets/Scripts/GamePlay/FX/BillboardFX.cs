using UnityEngine;
using Game.Core;

namespace Game {
    public class BillboardFX : MonoBehaviour
    {
        private Transform camTransform;

        private void Start()
        {
            camTransform = Camera.main.transform;
            //handle game end
            EventBus<GameEndEvent>.AddListener(HandleGameEnd);
        }

        private void Update()
        {
            //look towards cam transform
            Vector3 dir = camTransform.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.up);
        }

        private void OnDestroy()
        {
            EventBus<GameEndEvent>.RemoveListener(HandleGameEnd);
        }

        //======== Handle Game End ========
        private void HandleGameEnd(GameEndEvent eventData)
        {
            enabled = false;
        }
    }
}
