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
        Debug.Log("Patrolling");
        return;
    }

    public void OnExecute()
    {
        if (_hunter.target != null)
        {
            Vector3 targetPosition = _hunter.target.Position;

            if (Vector3.Distance(targetPosition, _hunter.transform.position) <= _hunter._visionRadius) // Posiblemente tenga que cambiar para integrar FOV y LOS
            {
                _manager.SetState<HunterChase>();
                return;
            }


            // LOGICA PARA QUE LOS OTROS ENEMIGOS PASEN A HunterSearch DEL ULTIMO NODO EN EL QUE SE VIO AL JUGADOR
            /*
             if ("Hunter.State == "TargetFound"" && Vector3.Distance(targetPosition, _hunter.transform.position) >= _hunter._visionRadius) // La primera parte esta entre comillas porque todavia estoy pensando como implementarlo
            {
                _manager.SetState<HunterSearch>();
                return;
            }
             */
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
        Debug.Log("NotPatrolling");
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
