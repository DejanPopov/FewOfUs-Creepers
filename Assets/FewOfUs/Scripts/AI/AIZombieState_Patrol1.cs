using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieState_Patrol1 : AIZombieState
{
    //Inspector assigned
    [SerializeField] AIWaypointNetwork waypoinyNetwork = null;
    [SerializeField] bool randomCOntrol = false;
    [SerializeField] int currentWaypoint = 0;
    [SerializeField] [Range(0.0f, 3.0f)] float speed = 1.0f;
    public override AIStateType getStateType()
    {
        return AIStateType.Partol;
    }

    public override AIStateType OnUpdate()
    {
        return AIStateType.Partol;
    }
}
