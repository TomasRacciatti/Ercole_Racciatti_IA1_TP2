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
        //Debug.Log(_hunter.name + ": Searching");
        _hunter.pathList = null;
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

        
        if (_hunter.pathList == null || _hunter.pathList.Count == 0)
        {
            _hunter.pathList = PathFinding.CalculatePathBFS(_hunter.startingNode, NodeManager.Instance.targetNode);
            _hunter.currentPathIndex = 0;
            lastTargetNode = NodeManager.Instance.targetNode;
        }
        
        if (NodeManager.Instance.targetNode != lastTargetNode)
        {
            lastTargetNode = NodeManager.Instance.targetNode;

            _hunter.pathList = PathFinding.CalculatePathBFS(_hunter.startingNode, NodeManager.Instance.targetNode);
            _hunter.currentPathIndex = 0;
        }


        PathFinding.MoveAlongPath(_hunter, _hunter.pathList);

        /* agregar logica que mira a los costados cuando llega hasta que se acabe el timer
        
        if (currentPathIndex >= _hunter.pathList.Count)
        {
            return;
        }
        */
    }

    public void OnSleep()
    {
        _hunter.currentPathIndex = 0;
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
