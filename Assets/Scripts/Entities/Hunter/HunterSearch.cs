using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSearch : IState
{
    private Hunter _hunter;
    private FSM _manager;

    public void OnAwake()
    {
        Debug.Log(_hunter.name + ": Searching");
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

        _hunter.pathList = PathFinding.CalculatePathBFS(_hunter.startingNode, NodeManager.Instance.targetNode);
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
