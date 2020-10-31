using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

        if (currentState)
        {
            currentState.OnDestinationReached(true);
        }
    }

}
 