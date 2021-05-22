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
        if (zombieStateMachine == null)
        {
            return AIStateType.Idle;
        }
        if (zombieStateMachine.VisualThreat.typeT == AITargetType.Visual_Player)
        {
            zombieStateMachine.SetTarget(zombieStateMachine.VisualThreat);
            return AIStateType.Pursuit;
        }
        if (zombieStateMachine.VisualThreat.typeT == AITargetType.Visual_Light)
        {
            zombieStateMachine.SetTarget(zombieStateMachine.VisualThreat);
            return AIStateType.Alerted;
        }
        if (zombieStateMachine.AudioThreat.typeT == AITargetType.Audio)
        {
            zombieStateMachine.SetTarget(zombieStateMachine.AudioThreat);
            return AIStateType.Alerted;
        }
        if (zombieStateMachine.VisualThreat.typeT == AITargetType.Visual_Food)
        {
            zombieStateMachine.SetTarget(zombieStateMachine.VisualThreat);
            return AIStateType.Pursuit;
        }
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            return AIStateType.Partol;
        }

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
        zombieStateMachine.attackTypeA = 0;
        zombieStateMachine.ClearTarget();
    }
    public override AIStateType getStateType()
    {
        if (zombieStateMachine == null)
        {
            return AIStateType.Idle;
        }
        if (zombieStateMachine.VisualThreat.typeT == AITargetType.Visual_Player)
        return AIStateType.Idle;
    }

}
