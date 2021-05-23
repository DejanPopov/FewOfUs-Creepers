using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombieState_Patrol1 : AIZombieState
{
    //Inspector assigned
    [SerializeField] AIWaypointNetwork waypoinyNetwork = null;
    [SerializeField] bool randomPatrol = false;
    [SerializeField] int currentWaypoint = 0;
    [SerializeField] [Range(0.0f, 3.0f)] float speed = 1.0f;

    public override AIStateType getStateType()
    {
        return AIStateType.Partol;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering partol state");
        base.OnEnterState();
        if (zombieStateMachine == null)
        {
            return;
        }

        zombieStateMachine.NavAgentControll(true, false);
        zombieStateMachine.speedS = speed;
        zombieStateMachine.seekingS = 0;
        zombieStateMachine.feddingF = false;
        zombieStateMachine.attackTypeA = 0;

        if (zombieStateMachine.targetType != AITargetType.Waypoint)
        {
            zombieStateMachine.ClearTarget();

            if (waypoinyNetwork != null && waypoinyNetwork.waypoints.Count > 0)
            {
                if (randomPatrol)
                {
                    currentWaypoint = Random.Range(0, waypoinyNetwork.waypoints.Count - 1);
                }

                Transform waypoint = waypoinyNetwork.waypoints[currentWaypoint];
                if (waypoint != null)
                {
                    zombieStateMachine.SetTarget(AITargetType.Waypoint, null, waypoint.position,
                        Vector3.Distance(zombieStateMachine.transform.position, waypoint.position));
                }

                zombieStateMachine.navAgentN.SetDestination(waypoint.position);

            }
        }

        zombieStateMachine.navAgentN.Resume();
    }

    public override AIStateType OnUpdate()
    {
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
        if (zombieStateMachine.VisualThreat.typeT == AITargetType.Visual_Light)
        {
            zombieStateMachine.SetTarget(zombieStateMachine.AudioThreat);
            return AIStateType.Alerted;
        }

        return AIStateType.Partol;
    }
}

