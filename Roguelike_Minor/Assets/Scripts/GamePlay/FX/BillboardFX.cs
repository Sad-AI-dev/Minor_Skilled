using UnityEngine;

namespace Game {
    public class BillboardFX : MonoBehaviour
    {
        private Transform camTransform;

        private void Start()
        {
            camTransform = Camera.main.transform;
        }

        private void Update()
        {
            //look towards cam transform
            Vector3 dir = camTransform.position - transform.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.up);
        }
    }
}
