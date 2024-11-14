using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterPatrol : IState
{
    private Hunter _hunter;
    private FSM _manager;

    private int _targetPoint = 0;
    private bool _isFirstPatrol = true;
    private bool _hasReturned = true;
    private Node returnNode;


    public void OnAwake()
    {
        if (!_isFirstPatrol)
        {
            _hunter.pathList = PathFinding.CalculatePathAStar(_hunter.startingNode, returnNode);
            _hunter.currentPathIndex = 0;
        }
        return;
    }

    public void OnExecute()
    {
        if (!_hasReturned)
        {
            PathFinding.MoveAlongPath(_hunter, _hunter.pathList);

            if (_hunter.currentPathIndex >= _hunter.pathList.Count)
            {
                _hasReturned = true;
            }
        }


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


        if (_hasReturned)
        {
            
            Node nextPatrolPoint = _hunter.patrolPoints[_targetPoint];

            if (_hunter.pathList == null || _hunter.currentPathIndex >= _hunter.pathList.Count)
            {
                _hunter.pathList = PathFinding.CalculatePathAStar(_hunter.startingNode, nextPatrolPoint);
                _hunter.currentPathIndex = 0;
            }

            PathFinding.MoveAlongPath(_hunter, _hunter.pathList);

            if (_hunter.currentPathIndex >= _hunter.pathList.Count)
            {
                ChangeWaypoint();
            }
            

            // CAMBIAR ESTA LOGICA POR UNA QUE USE PATH FINDING

            /*
            Vector3 patrolPointPosition = _hunter.patrolPoints[_targetPoint].transform.position;
           
            Vector3 desiredVelocity = SteeringBehaviours.Seek(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, patrolPointPosition, _hunter._steeringForce);
            _hunter.SetVelocity(desiredVelocity);

            _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;

            if (Vector3.Distance(_hunter.transform.position, patrolPointPosition) < 0.5f)
            {
                ChangeWaypoint();
            }
            */
        }
    }

    public void OnSleep()
    {
        _isFirstPatrol = false;
        _hasReturned = false;

        returnNode = _hunter.patrolPoints[_targetPoint];

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

        if (_targetPoint >= _hunter.patrolPoints.Count)
        {
            _targetPoint = 0;
        }
    }
}
