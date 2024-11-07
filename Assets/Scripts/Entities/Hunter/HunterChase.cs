using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterChase : IState
{
    private Hunter _hunter;
    private FSM _manager;

    public void OnAwake()
    {
        Debug.Log("Chasing");
        return;
    }

    public void OnExecute()
    {
        Player target = null;

        float distance = Vector3.Distance(_hunter.target.Position, _hunter.transform.position);

        if (distance < _hunter.VisionRadius * 1.2f)
        {
            target = _hunter.target;
        }

        if (target != null)
        {
            Vector3 targetPosition = target.Position;
            Vector3 targetVelocity = target.Velocity;


            if (Vector3.Distance(targetPosition, _hunter.transform.position) < _hunter.destroyDistance)
            {
                // GAME OVER
                return;
            }

            else
            {
                _hunter.SetVelocity(SteeringBehaviours.Pursuit(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, targetPosition, targetVelocity, _hunter._steeringForce, _hunter.maxFutureTime));

                _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;
            }

            /*
            else if ("Hunter.State == "TargetFound"" && !AIUtility.IsInFOV(_hunter, targetPosition, _hunter.obstacleMask))
            {
                _manager.SetState<HunterSearch>();
                return;
            }
            */
            if (!GameManager.Instance.playerFound && !AIUtility.IsInFOV(_hunter, targetPosition, _hunter.obstacleMask)) // El segundo chequeo tal vez no es necesario porque el GameManager.Instance.playerFound ya utiliza el IsInFOV
            {
                _manager.SetState<HunterPatrol>();
                return;
            }
        }

        if (target == null)
        {
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
