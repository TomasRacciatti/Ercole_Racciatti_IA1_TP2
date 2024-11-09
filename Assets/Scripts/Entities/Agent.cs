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
    protected int rotationState;
    [SerializeField] protected float lookAroundSpeed = 45f;
    public bool lookingAround = false;
    private Quaternion targetRotation;
    private bool rotationStateChanged = true;

    [Range(0f, 1f)] public float _steeringForce;
    public float maxFutureTime = 2f;
    public LayerMask obstacleMask;
    [SerializeField] protected float _visionRadius;
    [SerializeField] protected float _visionAngle;

    public List<Node> pathList = new List<Node>();
    public Node startingNode;
    public int currentPathIndex = 0;

    public void SetVelocity(Vector3 force)
    {
        _directionalVelocity = Vector3.ClampMagnitude(_directionalVelocity + force, speed);
    }


    public void LookAround()
    {
        Vector3 rotationDirection = Vector3.zero;

        if (rotationStateChanged)
        {
            if (rotationState == 0)
            {
                targetRotation = Quaternion.LookRotation(transform.forward);
            }
            else if (rotationState == 1)
            {
                targetRotation = Quaternion.LookRotation(Quaternion.Euler(0, 90f, 0) * transform.forward);
            }
            else if (rotationState == 2)
            {
                targetRotation = Quaternion.LookRotation(Quaternion.Euler(0, -90f, 0) * transform.forward);
            }
            else if (rotationState == 3)
            {
                targetRotation = Quaternion.LookRotation(Quaternion.Euler(0, -90f, 0) * transform.forward);
            }
            else if (rotationState == 4)
            {
                targetRotation = Quaternion.LookRotation(Quaternion.Euler(0, 90f, 0) * transform.forward);
            }
        }

        rotationStateChanged = false;
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lookAroundSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 2f)
        {
            rotationState = (rotationState + 1) % 5;
            rotationStateChanged = true;
        }
    }
}
