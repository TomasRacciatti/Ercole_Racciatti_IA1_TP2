using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;
    public virtual Vector3 Velocity => Vector3.zero;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }
}
