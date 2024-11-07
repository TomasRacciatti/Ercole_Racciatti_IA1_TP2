using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Entity
{
    public override Vector3 Velocity { get => _directionalVelocity; }
    public override Vector3 Position { get => transform.position; }
    public float Speed { get => _speedPropery; set => _speedPropery = value; } // Preguntar: Conviene usar una variable publica o hacer esta propiedad?
    public virtual float VisionRadius => _visionRadius;
    public virtual float VisionAngle => _visionAngle;

    [Header("Agent")]
    public Vector3 _directionalVelocity;
    [SerializeField] protected float _speedPropery; // Preguntar: Conviene usar una variable publica o hacer esta propiedad?
    public float speed;
    [SerializeField] protected float rotationSpeed;
    [Range(0f, 1f)] public float _steeringForce;
    public float maxFutureTime = 2f;
    public LayerMask obstacleMask;
    [SerializeField] protected float _visionRadius;
    [SerializeField] protected float _visionAngle;

    public void SetVelocity(Vector3 force)
    {
        _directionalVelocity = Vector3.ClampMagnitude(_directionalVelocity + force, speed);
    }
}
