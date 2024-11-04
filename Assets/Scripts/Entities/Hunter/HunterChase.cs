using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterChase : IState
{
    private Hunter _hunter;
    private FSM _manager;

    public void OnAwake()
    {
        return;
    }

    public void OnExecute()
    {
        throw new System.NotImplementedException();
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
