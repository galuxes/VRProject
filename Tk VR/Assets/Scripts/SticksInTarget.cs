using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SticksInTarget : MonoBehaviour
{
    [SerializeField] private Rigidbody parentRB;
    [SerializeField] private Collider SharpBit;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            parentRB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
