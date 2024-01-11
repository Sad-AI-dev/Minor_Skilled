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

        //active state vars
        private bool active;
        private List<Interactor> inactiveInteractors;

        private void Awake()
        {
            interactors = new List<Interactor>();
            inactiveInteractors = new List<Interactor>();
            active = true;
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
                if (active) {
                    interactors.Add(interactor);
                    interactor.AddInteractable(this);
                }
                else {
                    inactiveInteractors.Add(interactor);
                }
            }
        }

        private void RemoveFromInteractor(GameObject obj)
        {
            if (obj.TryGetComponent(out Interactor interactor)) {
                if (active) {
                    interactors.Remove(interactor);
                    interactor.RemoveInteractable(this);
                }
                else {
                    inactiveInteractors.Remove(interactor);
                }
            }
        }

        //============ Set Active State ===========
        public void SetActive(bool state)
        {
            if (active != state)
            {
                active = state;
                //handle enable
                if (active) { HandleEnable(); }
                //handle disable
                else { HandleDisable(); }
            }
        }

        private void HandleEnable()
        {
            foreach (Interactor interactor in inactiveInteractors) {
                interactors.Add(interactor);
                interactor.AddInteractable(this);
            }
            //reset inactive interactors
            inactiveInteractors.Clear();
        }

        private void HandleDisable()
        {
            //copy interactors
            inactiveInteractors.AddRange(interactors);
            //remove from all interactors
            RemoveFromAllInteractors();
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
            interactors.Clear();
        }
    }
}
