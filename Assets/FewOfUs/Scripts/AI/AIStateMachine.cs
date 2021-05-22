using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
//using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;


//AI states enumeration
public enum AIStateType
{
    None,
    Idle,
    Alerted,
    Partol,
    Attack,
    Feeding,
    Pursuit,
    Dead
}
//AI Target
public enum AITargetType
{
   None,
   Waypoint,
   Visual_Player,
   Visual_Light,
   Visual_Food,
   Audio
}

public enum AITriggerEventType
{
    Enter,
    Stay,
    Exit
}

public struct AITarget
{
    //This is type of target
    private AITargetType type;
    //Collider
    private Collider collider;
    //Position in the world
    private Vector3 position;
    //Distance from player
    private float distance;
    //Time the target last ping'd
    private float time;

    public AITargetType typeT { get { return type; } }
    public Collider colliderC { get { return collider; } }
    public Vector3 positionP { get { return position; } }
    public float distanceD { get { return distance; }  set {distance = value;}}
    public float timeT { get { return time; } }

    public void Set(AITargetType t, Collider c, Vector3 p, float d)
    {
        type = t;
        collider = c;
        position = p;
        distance = d;
        time = Time.time;
    }

    public void Clear()
    {
        type = AITargetType.None;
        collider = null;
        position = Vector3.zero;
        time = 0.0f;
        distance = Mathf.Infinity;
    }
}
public abstract class AIStateMachine : MonoBehaviour
{
    //Audio and visual threat
    public AITarget VisualThreat = new AITarget();
    public AITarget AudioThreat = new AITarget();

    //Dictionary for state types
    //When zombie enters some state we will store it in Dictionary thus knowing its state
    protected Dictionary<AIStateType, AIState> statesS = new Dictionary<AIStateType, AIState>();
    protected AITarget target = new AITarget();
    protected AIState currentState = null;
    protected int rootPositionRefCount = 0;
    protected int rootRotationRefCount = 0;

    //AI state machine is gona often start in idle
    [SerializeField]
    protected AIStateType currentStateType = AIStateType.Idle;

    [SerializeField]
    protected SphereCollider targetTrigger = null;

    [SerializeField]
    protected SphereCollider sensorTrigger = null;

    [SerializeField]
    [Range(0, 15)]
    protected float stoppingDistance = 1.0f;

    //Cashce reference
    protected Animator      animator =  null;
    protected NavMeshAgent  navAgent =  null;
    protected Collider      collider =  null;
    protected Transform     transformT = null;

    public Animator animatorA { get {return animator; } }
    public NavMeshAgent navAgentN { get{ return navAgent; }}

    public Vector3 sensorPosition
    {
        get
        {
            if (sensorTrigger == null)
            {
                return Vector3.zero;
            }

            Vector3 point = sensorTrigger.transform.position;
            point.x += sensorTrigger.center.x * sensorTrigger.transform.lossyScale.x;
            point.y += sensorTrigger.center.y * sensorTrigger.transform.lossyScale.y;
            point.z += sensorTrigger.center.z * sensorTrigger.transform.lossyScale.z;
            return point;
        }
    }

    //Calculate raidus in 3D space of sensor trigger
    public float sensorRadius
    {
        get
        {
            if (sensorTrigger == null)
            {
                return 0.0f;
            }

            float radius = Mathf.Max(sensorTrigger.radius * sensorTrigger.transform.lossyScale.x,
                sensorTrigger.radius * sensorTrigger.transform.lossyScale.y);

            return Mathf.Max(radius, sensorTrigger.radius * sensorTrigger.transform.lossyScale.z);
        }
    }

    public bool useRootPosition { get { return rootPositionRefCount > 0; } } // It was <
    public bool useRootRotation { get { return rootRotationRefCount > 0; } } // It was <


    //Cashe all components on the game object
    protected virtual void Awake()
    {
        transformT = transform;
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();

        //Activate sensor trigger and collider
        if (GameSceneManager.instanceI != null)
        {
            if (collider) 
            {
                GameSceneManager.instanceI.RegisterAIStateMachine(collider.GetInstanceID(), this);
            }

            if(sensorTrigger)
            {
                GameSceneManager.instanceI.RegisterAIStateMachine(sensorTrigger.GetInstanceID(), this);
            }
        }

    }

    protected virtual void Start()
    {
        if (sensorTrigger != null)
        {
            AISensor script = sensorTrigger.GetComponent<AISensor>();
            if (script != null)
            {
                script.parentStateMachines = this;
            }
        }

        //Array for states
        AIState[] states = GetComponents<AIState>();
        //Loop states and put them ion Dictionary
        foreach  (AIState state in states)
        {
            if (state != null && !statesS.ContainsKey(state.getStateType()));
            {
                statesS[state.getStateType()] = state;

                state.SetStateMachine(this);
            }
        }

        if (statesS.ContainsKey(currentStateType))
        {
            currentState = statesS[currentStateType];
            currentState.OnEnterState();
        }
        else
        {
            currentState = null;
        }

        if (animator)
        {
            AIStateMachineLink[] scripts = animator.GetBehaviours<AIStateMachineLink>();

            foreach (AIStateMachineLink script in scripts)
            {
                script.stateMachine = this;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        VisualThreat.Clear();
        AudioThreat.Clear();

        //If we have valid target, calculate target distance
        if (target.typeT != AITargetType.None)
        {
            target.distanceD = Vector3.Distance(transform.position, target.positionP);
        }
    }

    //This is trigger sphere on Jill
    public void SetTarget(AITargetType t, Collider c, Vector3 p, float d)
    {
        target.Set(t, c, p, d);

        if (targetTrigger != null)
        {
            targetTrigger.radius = stoppingDistance;
            targetTrigger.transform.position = target.positionP;
            targetTrigger.enabled = true;
        }
    }

    public void SetTarget(AITarget t)
    {
        target = t;

        if (targetTrigger != null)
        {
            targetTrigger.radius = stoppingDistance;
            targetTrigger.transform.position = target.positionP;
            targetTrigger.enabled = true;
        }
    }

    public void SetTarget(AITargetType t, Collider c, Vector3 p, float d, float s)
    {
        target.Set(t, c, p, d);

        if (targetTrigger != null)
        {
            targetTrigger.radius = s;
            targetTrigger.transform.position = target.positionP;
            targetTrigger.enabled = true;
        }
    }

    //Clear target when in no use
    public void ClearTarget()
    {
        target.Clear();

        if (targetTrigger != null)
        {
            targetTrigger.enabled = false;
        }
    }

    protected virtual void Update()
    {
        if (currentState == null)
        {
            return;
        }

        AIStateType newStateType = currentState.OnUpdate();

        if (newStateType != currentStateType)
        {
            AIState newState = null;

            if (statesS.TryGetValue(newStateType, out newState))
            {
                currentState.OnEnterState();
                newState.OnEnterState();
                currentState = newState;
            }
            else 
            if (statesS.TryGetValue(AIStateType.Idle, out newState))
            {
                currentState.OnEnterState();
                newState.OnEnterState();
                currentState = newState;
            }

            currentStateType = newStateType;
        }
    }
    //Called by physics system when enters main collidrs and its trigger
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (targetTrigger == null || other != targetTrigger)
        {
            return;
        }
        //Notify child state
        if (currentState)
        {
            currentState.OnDestinationReached(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (targetTrigger == null || targetTrigger != other)
        {
            return;
        }
        //Notify child state
        if (currentState)
        {
            currentState.OnDestinationReached(false);
        }
    }

    //Sensor is gona call this method
    public virtual void OnTriggerEvent(AITriggerEventType type, Collider other)
    {
        if (currentState != null)
        {
            currentState.OnTriggerEvent(type, other);
        }
    }

    protected virtual void OnAnimatorMove()
    {
        if (currentState != null)
        {
            currentState.OnAnimatorUpdated();
        }
    }

    //Notify currently active state to contact animator
    protected virtual void OnAnimatorIK(int layerIndex)
    {
        if (currentState != null)
        {
            currentState.OnAnimatorIKUpdated();
        }
    }

    public void NavAgentControll(bool positionUpdate, bool rotationUpdate)
    {
        if (navAgent)
        {
            navAgent.updatePosition = positionUpdate;
            navAgent.updateRotation = rotationUpdate;
        }
    }

    public void AddRootMotionRequest( int rootPosition, int rootRotatation)
    {
        rootPositionRefCount += rootPosition;
        rootRotationRefCount += rootRotatation;
    }
}