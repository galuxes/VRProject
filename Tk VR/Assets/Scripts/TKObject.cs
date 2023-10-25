using System;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Pooling;

namespace UnityEngine.XR.Interaction.Toolkit
{
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Outline))]
    [AddComponentMenu("XR/XR TK Interactable", 11)]
    public class TKObject : XRBaseInteractable
    {
        private Rigidbody m_Rigidbody;
        private Outline m_Outline;
        private Transform targetPosition;
        [SerializeField]private GameObject targetObj;
        [SerializeField] private float tkForce = 1f, distanceToStop = 0.05f, rotationalSpeed = 0.1f, expulsionForce = 50f;
        private bool hovering;
        private float mass;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Outline = GetComponent<Outline>();
            mass = m_Rigidbody.mass;
            
            targetObj = new GameObject();
            targetObj = Instantiate(targetObj, transform);
            targetPosition = targetObj.transform;
        }

        private void Update()
        {
            if (!hovering) return;
            float distance = Vector3.Distance(transform.position, targetPosition.position);
            if (distance > distanceToStop)
            {
                m_Rigidbody.velocity = (targetPosition.position - transform.position).normalized * ((distance/tkForce)/mass);
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetPosition.rotation, rotationalSpeed/mass);
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            m_Outline.enabled = true;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            m_Outline.enabled = false;
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            targetPosition.position = args.interactorObject.transform.position;

            /*if (interactorsSelecting.Count == 1)
            {
                Grab(args.interactorObject.transform);
            }*/
        }

        protected override void OnDeactivated(DeactivateEventArgs args)
        {
            m_Rigidbody.velocity = args.interactorObject.transform.forward * expulsionForce;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (interactorsSelecting.Count == 1)
            {
                Hover(args.interactorObject.transform);
            }
            else if(interactorsSelecting.Count == 2)
            {
                //TODO::insert two handed mode
            }
        }
        
        private void Hover(Transform transform)
        {
            hovering = true;
            targetPosition.parent = transform;
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            if (interactorsSelecting.Count == 0)
            {
                HoverDrop();
            }
        }

        private void HoverDrop()
        {
            hovering = false;
            targetPosition.rotation = transform.rotation;
            
            targetPosition.position = transform.position;
            targetPosition.SetParent(transform);
        }

        private Transform ogParent;
        
        protected virtual void Grab(Transform interactor)
        {
            HoverDrop();
            
            ogParent = transform.parent;
            transform.SetParent(null);

            // Reset detach velocities
            //m_DetachVelocity = Vector3.zero;
            //m_DetachAngularVelocity = Vector3.zero;
            
            transform.position = interactor.position;
            transform.rotation = interactor.rotation;
            transform.localScale = interactor.localScale;
            transform.parent = interactor;
            m_Rigidbody.useGravity = false;
        }
    }
    
    
}
