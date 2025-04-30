using System;
using UnityEngine;

namespace Base
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        void OnInteract(IInteractable target);
        
        
        void OnExitInteract(IInteractable target);
    }
    
    public abstract class AInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        public bool CanInteract => _isInteractable;
        protected void SetCanInteract(bool isInteractable) => this._isInteractable = isInteractable;
        /// <summary>
        /// Do something when the object has collision with the object
        /// </summary>
        /// <param name="target"></param>
        public abstract void OnInteract(IInteractable target);

        /// <summary>
        /// DoSomething when the object exits the trigger collider
        /// </summary>
        /// <param name="target"></param>
        public abstract void OnExitInteract(IInteractable target);
        
        protected void OnTriggerEnter2D(Collider2D  other)
        {
            if(!other) return;
            if (!other.transform.TryGetComponent(out IInteractable interactable)) return;
            if (interactable.CanInteract)
            {
                interactable.OnInteract(this);
            }
        }
        
        protected void OnTriggerExit2D(Collider2D other)
        {
            if(!other) return;
            if (!other.transform.TryGetComponent(out IInteractable interactable)) return;
            if (interactable.CanInteract)
            {
                interactable.OnExitInteract(this);
            }
        }
    }
    
    /// <summary>
    /// class AInteractableWithStay is used to handle the OnStayInteract event
    /// </summary>
    public abstract class AInteractableWithStay : AInteractable
    {
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (!other) return;
            if (!other.transform.TryGetComponent(out IInteractable interactable)) return;
            if (interactable.CanInteract && interactable is AInteractableWithStay stayInteractable)
            {
                stayInteractable.OnStayInteract(this);
            }
        }
        
        
              
        /// <summary>
        /// Do something when the object stay in the trigger collider
        /// </summary>
        /// <param name="target"></param>
        public abstract void OnStayInteract(IInteractable target);
    }
}