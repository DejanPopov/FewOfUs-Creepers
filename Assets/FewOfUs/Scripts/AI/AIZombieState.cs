using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIZombieState : AIState
{
    protected int playerLayerMask = -1;
    protected int bodyPartLayer = -1;
    protected AIZombieStateMachine zombieStateMachine = null;

    private void Awake()
    {
        playerLayerMask = LayerMask.GetMask("Player","AI Body Part") + 1;
        bodyPartLayer = LayerMask.GetMask("AI Body Part");
    }
    public virtual void SetStateMachine(AIStateMachine stateMachine)
    {
        if (stateMachine.GetType() == typeof(AIZombieStateMachine))
        {
            base.SetStateMachine(stateMachine);
            zombieStateMachine = (AIZombieStateMachine)stateMachine;
        }
    }

    public override void OnTriggerEvent(AITriggerEventType eventType, Collider other)
    {
        
        if (zombieStateMachine == null)
        {
            return;
        }

        if (eventType != AITriggerEventType.Exit)
        {
            AITargetType curType = zombieStateMachine.VisualThreat.typeT;

            if (other.CompareTag("Player"))
            {
                float distance = Vector3.Distance(zombieStateMachine.sensorPosition, other.transform.position);
                if (curType != AITargetType.Visual_Player || (curType == AITargetType.Visual_Player &&
                    distance < zombieStateMachine.VisualThreat.distanceD))
                {
                    RaycastHit hitInfo;
                    if (ColliderIsVisible(other, out hitInfo, playerLayerMask))
                    {
                        zombieStateMachine.VisualThreat.Set(AITargetType.Visual_Player, other, other.transform.position,
                            distance);
                    }
                }
            }
            else
            {
                //FlashLight
                if (other.CompareTag("Flash Light") && curType != AITargetType.Visual_Player)
                {
                    BoxCollider flashLightTrigger = (BoxCollider)other;
                    float distanceToThreat = Vector3.Distance(zombieStateMachine.sensorPosition, flashLightTrigger.transform.position);
                    float zSize = flashLightTrigger.size.z * flashLightTrigger.transform.lossyScale.z;
                }
            }
        }
       // base.OnTriggerEvent(eventType, other);
    }

    protected virtual bool ColliderIsVisible(Collider other, out RaycastHit hitInfo, int layerMask = -1)
    {
        hitInfo = default;
        if (zombieStateMachine == null)
        {
            return false;
        }

        Vector3 head = machineM.sensorPosition;
        Vector3 direction = other.transform.position = head;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle > zombieStateMachine.fovF * 0.5f)
        {
            return false;
        }
        RaycastHit[] hits = Physics.RaycastAll(head, direction.normalized, zombieStateMachine.sensorRadius
            * zombieStateMachine.sightS, layerMask);

        //Find closest collider that is not AI part body
        float closestColliderDistance = float.MaxValue;
        Collider closestCollider = null;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hitInfo.transform.gameObject.layer == bodyPartLayer)
            {
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
