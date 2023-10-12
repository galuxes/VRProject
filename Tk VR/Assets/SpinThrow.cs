using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinThrow : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float spinModifier;
    [SerializeField] private bool isFlying = false;
    
    
    private void Update()
    {
        if (isFlying)
        {
            transform.Rotate(Vector3.right * spinModifier);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            isFlying = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFlying = true;
        }
    }
}
