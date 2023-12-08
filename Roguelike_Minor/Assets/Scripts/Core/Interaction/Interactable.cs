using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core.GameSystems {
    public class Interactable : MonoBehaviour
    {
        [Header("UI Settings")]
        [Tooltip("setting for what to display in the interaction label HUD element.\n" +
            "default is \"[Keybind] to \". label will be pasted after the default string.")]
        public string label = "interact";

        [Header("On Interact Event")]
        public UnityEvent<Interactor> onInteract;

        private List<Interactor> interactors;

        private void Awake()
        {
            interactors = new List<Interactor>();
        }

        //============ trigger events ============
        private void OnTriggerEnter(Collider other) {
            AddToInteractor(other.gameObject);
        }
        private void OnTriggerExit(Collider other) {
            RemoveFromInteractor(other.gameObject);
        }

        //========= 2D trigger events =========
        private void OnTriggerEnter2D(Collider2D collision) {
            AddToInteractor(collision.gameObject);
        }
        private void OnTriggerExit2D(Collider2D collision) {
            RemoveFromInteractor(collision.gameObject);
        }

        //====== on find interactor reactions ======
        private void AddToInteractor(GameObject obj)
        {
            if (obj.TryGetComponent(out Interactor interactor)) {
                interactors.Add(interactor);
                interactor.AddInteractable(this);
            }
        }

        private void RemoveFromInteractor(GameObject obj)
        {
            if (obj.TryGetComponent(out Interactor interactor)) {
                interactors.Remove(interactor);
                interactor.RemoveInteractable(this);
            }
        }

        //============ handle disable/destroy ============
        private void OnDestroy()
        {
            RemoveFromAllInteractors();
        }

        private void OnDisable()
        {
            RemoveFromAllInteractors();
        }

        private void RemoveFromAllInteractors()
        {
            foreach (Interactor interactor in interactors) {
                interactor.RemoveInteractable(this);
            }
        }
    }
}
