using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core.GameSystems {
    public class Interactor : MonoBehaviour
    {
        [Header("Events")]
        [Tooltip("triggers when the interactor comes in range of the first interactable")]
        public UnityEvent<Interactable> onCanInteract;
        [Tooltip("triggers when a new interactable comes in range of the interactor")]
        public UnityEvent<Interactable> onInteractablesChanged;
        [Tooltip("triggers when the interactor leaves the range of the last interactable")]
        public UnityEvent onStopCanInteract;

        [Header("Base Event")]
        public UnityEvent onInteract;
        //vars
        private List<Interactable> interactables;
        [HideInInspector] public Agent agent;

        private void Start()
        {
            interactables = new List<Interactable>();
            agent = GetComponent<Agent>();
        }

        //=============== interactables management ===============
        public void AddInteractable(Interactable interactable)
        {
            if (!enabled || !interactable.enabled) { return; }
            interactables.Add(interactable);
            if (interactables.Count == 1) {
                onCanInteract?.Invoke(interactable);
            }
            //trigger find interactable
            if (interactables.Count > 1) { SortInteractables(); }
            onInteractablesChanged?.Invoke(interactables[0]);
        }

        public void RemoveInteractable(Interactable interactable)
        {
            interactables.Remove(interactable);
            if (interactables.Count == 0) { onStopCanInteract.Invoke(); }
            else
            {
                if (interactables.Count > 1) { SortInteractables(); }
                onInteractablesChanged?.Invoke(interactables[0]);
            }
        }

        //=============== interact with interactable ===============
        public void TryInteract()
        {
            if (interactables.Count > 0) {
                if (interactables.Count > 1) { //only sort if more than 1 interactable available
                    SortInteractables(); //interact with closest interactable
                }
                interactables[0].onInteract?.Invoke(this);
                onInteract?.Invoke();
            }
        }

        private void SortInteractables()
        {
            interactables.Sort((Interactable a, Interactable b) => 
                -Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position))
            );
        }

        //============== Handle Disable ===============
        private void OnDisable()
        {
            //clear interactables
            for (int i = interactables.Count - 1; i >= 0; i--)
            {
                RemoveInteractable(interactables[i]);
            }
        }
    }
}
