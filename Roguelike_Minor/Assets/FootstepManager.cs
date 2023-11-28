using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event footstepSFX;

    [SerializeField] private LayerMask Floorlayer;

    private CharacterController Controller;
    private void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    private void PlayFootstep()
    {
        footstepSFX.Post(gameObject);
    }

    private IEnumerator CheckGround()
    {
        while (true)
        {
            if (Controller.velocity != Vector3.zero && Physics.Raycast(
                    transform.position - new Vector3(0, 0.5f * Controller.height + 0.5f * Controller.radius, 0),
                    Vector3.down, out RaycastHit hit, 1f, Floorlayer))
            {
                if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                {
                    yield return StartCoroutine(SetFootstepSFXTerrain(terrain, hit.point));
                }
                else if (hit.collider.TryGetComponent<GameObject>(out GameObject obj))
                {
                    yield return StartCoroutine(SetFootstepSFXRender(obj));
                }

            }
        }
    }

    private IEnumerator SetFootstepSFXTerrain(Terrain terrain, Vector3 hitpoint)
    {
        return null;
    }
    
    private IEnumerator SetFootstepSFXRender(GameObject obj)
    {
        return null;
    }
    
    [System.Serializable]
    private class TextureSound
    {
        public Texture Albedo;
        public AK.Wwise.Switch WalkingSurface;
    }

}
