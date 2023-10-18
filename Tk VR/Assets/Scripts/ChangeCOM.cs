using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCOM : MonoBehaviour
{
    [SerializeField] private Rigidbody parentRb;
    private void Awake()
    {
        parentRb.centerOfMass = transform.localPosition;
    }
}
