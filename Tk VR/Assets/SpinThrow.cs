using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum Axis
{
    x,
    y,
    z
}

public class SpinThrow : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float spinModifier;
    [SerializeField] private bool isFlying = false;
    [SerializeField] private Axis axisOfRotation;
    private Vector3 axisVector3;

    private void Start()
    {
        axisVector3 = axisOfRotation switch
        {
            Axis.x => Vector3.right,
            Axis.y => Vector3.up,
            Axis.z => Vector3.forward,
            _ => axisVector3
        };
    }

    private void Update()
    {
        if (isFlying)
        {
            transform.Rotate(axisVector3 * spinModifier);
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
