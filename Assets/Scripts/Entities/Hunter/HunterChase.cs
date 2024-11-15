using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HunterChase : IState
{
    private Hunter _hunter;
    private FSM _manager;

    public void OnAwake()
    {
        //Debug.Log(_hunter.name + ": Chasing");
        return;
    }

    public void OnExecute()
    {
        Player target = null;

        float distanceToTarget = Vector3.Distance(_hunter.target.Position, _hunter.transform.position);

        if (distanceToTarget < _hunter.VisionRadius * 1.2f)
        {
            target = _hunter.target;
        }

        if (target != null)
        {
            Vector3 targetPosition = target.Position;
            Vector3 targetVelocity = target.Velocity;

            NodeManager.Instance.lastTargetNode = NodeManager.Instance.GetClosestNode(targetPosition);  // El hunter que ve al jugador settea el nodo mas cercano


            if (Vector3.Distance(targetPosition, _hunter.transform.position) < _hunter.destroyDistance)
            {
                if(!GameManager.Instance.gameOver)
                {
                    GameManager.Instance.gameOver = true;
                    GameManager.Instance.TriggerGameOver();
                }
                return;
            }

            else
            {
                _hunter.SetVelocity(SteeringBehaviours.Pursuit(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, targetPosition, targetVelocity, _hunter._steeringForce, _hunter.maxFutureTime));

                _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;
            }

            
            if (GameManager.Instance.playerFound && !AIUtility.IsInFOV(_hunter, targetPosition, _hunter.obstacleMask))
            {
                _manager.SetState<HunterSearch>();
                return;
            }
            
            if (!GameManager.Instance.playerFound)
            {
                _manager.SetState<HunterPatrol>();
                return;
            }
        }

        if (target == null)
        {
            if (GameManager.Instance.playerFound)
            {
                _manager.SetState<HunterSearch>();
                return;
            }

            _manager.SetState<HunterPatrol>();
                return;
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
}
