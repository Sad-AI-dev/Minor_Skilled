using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;

namespace Game.Core {
    public class Interactor : MonoBehaviour
    {
        [Header("Events")]
        [Tooltip("triggers when the interactor comes in range of the first interactable")]
        public UnityEvent onCanInteract;
        [Tooltip("triggers when the interactor leaves the range of the last interactable")]
        public UnityEvent onStopCanInteract;
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
            interactables.Add(interactable);
            if (interactables.Count == 1) {
                onCanInteract?.Invoke();
            }
        }

        public void RemoveInteractable(Interactable interactable)
        {
            interactables.Remove(interactable);
            if (interactables.Count == 0) { onStopCanInteract.Invoke(); }
        }

        //=============== interact with interactable ===============
        public void TryInteract()
        {
            if (interactables.Count > 0) {
                if (interactables.Count > 1) { //only sort if more than 1 interactable available
                    SortInteractables(); //interact with closest interactable
                }
                interactables[0].onInteract?.Invoke(this);
            }
        }

        private void SortInteractables()
        {
            interactables.Sort((Interactable a, Interactable b) => 
                Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position))
            );
        }
    }
}
