using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSearch : IState
{
    private Hunter _hunter;
    private FSM _manager;

    private Node lastTargetNode;

    public void OnAwake()
    {
        _hunter.pathList = null;
        
        // Apenas entra calculo el path una sola vez. Si lastTargetNode no cambia, este es el camino que va a quedar

        if (_hunter.pathList == null || _hunter.pathList.Count == 0)
        {
            _hunter.pathList = PathFinding.CalculatePathAStar(_hunter, _hunter.startingNode, NodeManager.Instance.lastTargetNode);
            _hunter.currentPathIndex = 0;
            lastTargetNode = NodeManager.Instance.lastTargetNode;
        }

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
        }

        if (!GameManager.Instance.playerFound)
        {
            _manager.SetState<HunterPatrol>();
            return;
        }


        // Logica: ir al ultimo nodo donde se vio al jugador
        
        // Esta condicion es por si un cazador esta persiguiendo al jugador a traves del mapa
        // En ese caso, el "ultimo nodo" estaría cambiando porque un cazador lo ve moverse de nodo a nodo
        // Me obliga a calcular el path de vuelta
        if (NodeManager.Instance.lastTargetNode != lastTargetNode)
        {
            lastTargetNode = NodeManager.Instance.lastTargetNode;

            _hunter.pathList = PathFinding.CalculatePathAStar(_hunter, _hunter.startingNode, NodeManager.Instance.lastTargetNode);
            _hunter.currentPathIndex = 0;
        }


        PathFinding.MoveAlongPath(_hunter, _hunter.pathList);

        
        if (_hunter.currentPathIndex >= _hunter.pathList.Count)
        {
            if (GameManager.Instance.lastPlayerPosition != null)
            {
                Vector3 lastKnownPosition = GameManager.Instance.lastPlayerPosition;

                if (Vector3.Distance(_hunter.transform.position, lastKnownPosition) > 0.5f)
                {
                    Vector3 desiredVelocity = SteeringBehaviours.Seek(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, lastKnownPosition, _hunter._steeringForce);
                    _hunter.SetVelocity(desiredVelocity);
                    _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;
                }
                else
                {
                    _hunter.lookingAround = true;
                }
            }
            else
            {
                _hunter.lookingAround = true; // Por las dudas si la ultima posicion es nula, que vayan al ultimo nodo y la queden ahi
            }
        }
    }

    public void OnSleep()
    {
        _hunter.currentPathIndex = 0;
        _hunter.lookingAround = false;
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
