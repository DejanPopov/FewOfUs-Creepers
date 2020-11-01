using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIZombieState : AIState
{
    protected int playerLayerMask = -1;
    protected int bodyPartLayer = -1;

    private void Awake()
    {
        playerLayerMask = LayerMask.GetMask("Player","AI Body Part") + 1;
        bodyPartLayer = LayerMask.GetMask("AI Body Part");
    }

    public override void OnTriggerEvent(AITriggerEventType eventType, Collider other)
    {
        
        if (machineM == null)
        {
            return;
        }

        if (eventType != AITriggerEventType.Exit)
        {
            AITargetType curType = machineM.VisualThreat.typeT;

            if (other.CompareTag("Player"))
            {
                Vector3 distance = Vector3.Distance(machineM.sensorPosition, other.transform.position);
                if (curType != AITargetType.Visual_Player || (curType == AITargetType.Visual_Player &&
                    distance < machineM.VisualThreat.distanceD))
                {
                    RaycastHit hitInfo;
                    if (ColliderIsVisible(other, out hitInfo, playerLayerMask))
                    {
                        machineM.VisualThreat.Set(AITargetType.Visual_Player, other, other.transform.position,
                            distance)
                    }
                }
            }
        }
       // base.OnTriggerEvent(eventType, other);
    }

    protected virtual bool ColliderIsVisible(Collider other, out RaycastHit hitInfo, int layerMask = -1)
    {
        hitInfo = default;
        if (machineM == null || machineM.GetType() != typeof(AIZombieStateMachine))
        {
            return false;
        }
        AIZombieStateMachine zombieMachine = (AIZombieStateMachine)machineM;

        Vector3 head = machineM.sensorPosition;
        Vector3 direction = other.transform.position = head;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle > zombieMachine.fovF * 0.5f)
        {
            return false;
        }
        RaycastHit[] hits = Physics.RaycastAll(head, direction.normalized, machineM.sensorRadius
            * zombieMachine.sightS, layerMask);

        //Find closest collider that is not AI part body
        float closestColliderDistance = float.MaxValue;
        Collider closestCollider = null;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.distance < closestColliderDistance)
            {
                if (machineM != GameSceneManager.instanceI.GetAIStateMachine
                    (hit.rigidbody.GetInstanceID()))
                {
                    closestColliderDistance = hit.distance;
                    closestCollider = hit.collider;
                    hitInfo = hit;
                }
            }
            else
            {
                closestColliderDistance = hit.distance;
                closestCollider = hit.collider;
                hitInfo = hit;
            }
        }

        if (closestCollider && closestCollider.gameObject == other.gameObject)
        {
            return true;
        }

        return false;
    }
}
