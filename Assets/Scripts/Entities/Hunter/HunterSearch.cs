using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSearch : IState
{
    private Hunter _hunter;
    private FSM _manager;

    private int currentPathIndex = 0;

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

        if (_hunter.pathList == null || _hunter.pathList.Count == 0)
        {
            _hunter.pathList = PathFinding.CalculatePathBFS(_hunter.startingNode, NodeManager.Instance.targetNode);
            currentPathIndex = 0;
        }

        MoveAlongPath();

        //PathFinding.MoveAlongPath(_hunter, _hunter.pathList, currentPathIndex);

        /* agregar logica que mira a los costados cuando llega hasta que se acabe el timer
        
        if (currentPathIndex >= _hunter.pathList.Count)
        {
            return;
        }
        */
    }

    public void OnSleep()
    {
        currentPathIndex = 0;
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


  

    public void MoveAlongPath()
    {
        if (_hunter.pathList != null && currentPathIndex < _hunter.pathList.Count)
        {
            Node currentNode = _hunter.pathList[currentPathIndex];
            Vector3 nodePosition = currentNode.transform.position;

            Vector3 desiredVelocity = SteeringBehaviours.Seek(_hunter.transform.position, _hunter.speed, _hunter._directionalVelocity, nodePosition, _hunter._steeringForce);
            _hunter.SetVelocity(desiredVelocity);

            _hunter.transform.position += _hunter._directionalVelocity * Time.deltaTime;

            
            if (Vector3.Distance(_hunter.transform.position, currentNode.transform.position) < 0.5f)
            {
                currentPathIndex++;
            }
        }
        else
        {
            Debug.Log("Hunter: Path completed or no path available.");
        }
    }
   
}
