using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieState_Idle1 : AIZombieState
{
    //Random zombie choosing state
    [SerializeField] Vector2 idleTimeRange = new Vector2(10.0f, 60.0f);

    private float idleTime = 0.0f;
    private float timer = 0.0f;
    public override AIStateType OnUpdate()
    {

        return AIStateType.Idle;
    }
    public override void OnEnterState()
    {
        Debug.Log("Entering idle state");
        base.OnEnterState();
        if ( zombieStateMachine == null)
        {
            return;
        }

        idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
        timer = 0.0f;

        zombieStateMachine.NavAgentControll(true, false);
        zombieStateMachine.speedS = 0;
        zombieStateMachine.seekingS = 0;
        zombieStateMachine.feddingF = false;

    }
    public override AIStateType getStateType()
    {

        return AIStateType.Idle;
    }

}
