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
    [AddComponentMenu("XR/XR TK Interactable", 11)]
    public class TKObject : XRBaseInteractable
    {
        private Rigidbody m_Rigidbody;
        private Transform targetPosition;
        [SerializeField]private GameObject targetObj;
        [SerializeField] private float tkForce = 1f, distanceToStop = 0.05f, rotationalSpeed = 0.1f;
        //private bool hovering;
        private float mass;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            mass = m_Rigidbody.mass;
            
            targetObj = new GameObject();
            targetObj = Instantiate(targetObj, transform);
            targetPosition = targetObj.transform;
        }

        private void Update()
        {
            float distance = Vector3.Distance(transform.position, targetPosition.position);
            if (distance > distanceToStop)
            {
                m_Rigidbody.velocity = (targetPosition.position - transform.position).normalized * ((distance/tkForce)/mass);
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetPosition.rotation, rotationalSpeed/mass);
        }

        protected override void OnActivated(ActivateEventArgs args)
        {
            Destroy(gameObject);
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
            targetPosition.parent = transform;
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            if (interactorsSelecting.Count == 0)
            {
                Drop();
            }
        }

        private void Drop()
        {
            targetPosition.rotation = transform.rotation;
            
            targetPosition.position = transform.position;
            targetPosition.SetParent(transform);
        }
    }
    
    
}
