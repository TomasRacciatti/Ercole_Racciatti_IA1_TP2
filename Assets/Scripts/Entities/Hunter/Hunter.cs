using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{

    public float destroyDistance = 2f;
    public Transform[] patrolPoints;
    public FSM stateMachine;

    public Player target;
    public LayerMask playerLayerMask;

    private void Awake()
    {
        stateMachine = new();


        stateMachine.AddNewState<HunterSearch>().SetAgent(this);
        stateMachine.AddNewState<HunterPatrol>().SetAgent(this);
        stateMachine.AddNewState<HunterChase>().SetAgent(this);

        stateMachine.SetInitialState<HunterPatrol>();

    }


    private void Update()
    {
        stateMachine.OnUpdate();
        DetectPlayerInRange();

        if (_directionalVelocity != Vector3.zero)  // Hunter rotation to objective
        {
            Quaternion targetRotation = Quaternion.LookRotation(_directionalVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            return;
        }
    }


    private void DetectPlayerInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _visionRadius, playerLayerMask);

        if (hits.Length > 0)
        {
            target = hits[0].GetComponent<Player>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _visionRadius);
    }

}
