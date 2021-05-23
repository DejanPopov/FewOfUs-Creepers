using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieState_Patrol1 : AIZombieState
{
    public override AIStateType getStateType()
    {
        return AIStateType.Partol;
    }

    public override AIStateType OnUpdate()
    {
        return AIStateType.Partol;
    }
}
