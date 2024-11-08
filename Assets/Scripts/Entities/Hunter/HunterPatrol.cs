using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterPatrol : IState
{
    private Hunter _hunter;
    private FSM _manager;

    private int _targetPoint = 0;

    public void OnAwake()
    {
        //_hunter.pathList = PathFinding.CalculatePathBFS(_hunter.startingNode, _hunter.patrolPoints[_targetPoint].position);
        //_hunter.currentPathIndex = 0;
        
        return;
    }

    public void OnExecute()
    {
        if (_hunter.target != null)
        {
            Vector3 targetPosition = _hunter.target.Position;

            if (AIUtility.IsInFOV(_hunter, targetPosition, _hunter.obstacleMask))
            {

                _manager.SetState<HunterChase>();
                return;
            }
            
            if (GameManager.Instance.playerFound && !AIUtility.IsInFOV(_hunter, targetPosition, _hunter.obstacleMask))
            {
                _manager.SetState<HunterSearch>();
                return;
            }
        }

        if (GameManager.Instance.playerFound && _hunter.target == null) // Tengo que hacer este chequeo para el caso que lo vea un hunter cuando los otros nunca lo referenciaron. Cuando los otros hunters se acerquen mediante el search, deberian referenciar al jugador
        {
            _manager.SetState<HunterSearch>();
            return;
        }

        Vector3 desiredVelocity = SteeringBehaviours.Seek(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, _hunter.patrolPoints[_targetPoint].position, _hunter._steeringForce);
        _hunter.SetVelocity(desiredVelocity);

        _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;

        if (Vector3.Distance(_hunter.transform.position, _hunter.patrolPoints[_targetPoint].position) < 0.5f)
        {
            ChangeWaypoint();
        }
    }

    public void OnSleep()
    {
        return;
    }

    public void SetAgent(Agent agent)
    {
        _hunter = (Hunter)agent;
    }

    public void SetFSM(FSM manager)
    {
        _manager = manager;
    }


    void ChangeWaypoint()
    {
        _targetPoint++;

        if (_targetPoint >= _hunter.patrolPoints.Length)
        {
            _targetPoint = 0;
        }
    }
}
