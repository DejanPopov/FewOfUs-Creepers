using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieState_Idle1 : AIZombieState
{
    public override AIStateType OnUpdate()
    {
        Debug.Log("State type beeing fetched by state machine");
        return AIStateType.Idle;
    }
    public override AIStateType getStateType()
    {

        return AIStateType.Idle;
    }

}
