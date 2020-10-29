using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentRootMotion : MonoBehaviour
{
    //Option that we can assign Waypoint Network
    public AIWaypointNetwork WaypointNetwork = null;
    //Waypoint number
    public int CurrentIndex = 0;
    //For getting to other destinations
    public bool HasPath = false;
    public bool PathPending = false;
    public bool PathStale = false;
    //Path where zombies can climb to get to higher grounds or stand at that point
    //before zombie gives up and goes to other waypoint
    public NavMeshPathStatus PathStatus = NavMeshPathStatus.PathInvalid;

    //Animation jump curve
    public AnimationCurve JumpCurve = new AnimationCurve();

    private NavMeshAgent navAgent = null;
    //This is for zombie 
    private Animator animator = null;
    //private float originalMaxSpeed = 0;
    private float smoothAngle = 0.0f;
    public bool MixedMode = true;

    // Start is called before the first frame update
    void Start()
    {
        //NavMesh agent reference
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //Test for NoRootMotion
        /*
        if (navAgent)
        {
            originalMaxSpeed = navAgent.speed;
        }
        */

        //This code is to see for fun that if the animated enemy is not moving
        //but NavMeshAgent is moving then this is probably whats happend
        /*
        navAgent.updatePosition = false;
         */
        navAgent.updateRotation = false;
        

        if (WaypointNetwork == null)
        {
            return;
        }

        SetNextDestionation(false);
    }

    // Update is called once per frame
    void Update()
    {
        //For turning of zombie on map
        //int turnOnSpot;

        //Will show in inspector
        HasPath = navAgent.hasPath;
        PathPending = navAgent.pathPending;
        PathStale = navAgent.isPathStale;
        PathStatus = navAgent.pathStatus;

        //Trigonometry for angle
        Vector3 localDesiredVelocity = transform.InverseTransformVector(navAgent.desiredVelocity);
        //Multiply to get degress
        float angle = Mathf.Atan2(localDesiredVelocity.x, localDesiredVelocity.y) * Mathf.Rad2Deg;
        //80 degress in single second turning
        smoothAngle = Mathf.MoveTowardsAngle(smoothAngle, angle, 80.0f * Time.deltaTime);

        //How fast is zombie walking from it's point of view
        float speed = localDesiredVelocity.z;

        animator.SetFloat("Angle", smoothAngle);
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        if (navAgent.desiredVelocity.sqrMagnitude > Mathf.Epsilon)
        {
            if (!MixedMode || (MixedMode && Mathf.Abs(angle) < 80.0f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Locomotion")))
            {
                Quaternion lookRotation = Quaternion.LookRotation(navAgent.desiredVelocity, Vector3.up);
                //The 4th parameter should be Time.DeltaTime
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5.0f);
            }
        }

        //This code is for NoRootMotion
        /*
        //Cost between transforms for vector and velocity vector and normalize it
        Vector3 cross = Vector3.Cross(transform.forward, navAgent.desiredVelocity.normalized);
        float horizontal = (cross.y < 0) ? -cross.magnitude : cross.magnitude;
        horizontal = Mathf.Clamp(horizontal * 4.32f, -2.32f, 2.32f);

        //If agents speed drops to 0 it gets set to 0.1
        //This code 
        // Vector3.Angle(transform.forward, navAgent.desiredVelocity) > 20.0f)
        //is for when agent stops and is turning. It will turn into infinity if dont get Angle property
        if (navAgent.desiredVelocity.magnitude < 1.0f && 
            Vector3.Angle(transform.forward, navAgent.desiredVelocity) > 10.0f)
        {
            navAgent.speed = 0.1f;
            //Calculate turn of zombie
            turnOnSpot = (int)Mathf.Sign(horizontal);
        }
        else
        {
            navAgent.speed = originalMaxSpeed;
            turnOnSpot = 0;
        }

        //Damping smoothes the values (0.1f)
        animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", navAgent.desiredVelocity.magnitude, 0.1f, Time.deltaTime);
        //Trying to smoth the cornering animation
        animator.SetInteger("TurnOnSpot", turnOnSpot);
        */

        //This will not be used on zombies!
        //This will be used for agent to start coroutine called JUMP
        /*if (navAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(1.0f));
            return;
        }
        */

        //!HasPath will be little buggy to use when agent is jumping across platforms
        //We can use this || PathStatus == NavMeshPathStatus.PathPartial into
        //if () statement and then the agent will not go to invalid path that cant reach
        if ((/*!HasPath*/ navAgent.remainingDistance <= navAgent.stoppingDistance && !PathPending) ||
            PathStatus == NavMeshPathStatus.PathInvalid /*||
            PathStatus == NavMeshPathStatus.PathPartial*/)
        {
            SetNextDestionation(true);
        }
        else
        {
            //Stale path
            if (navAgent.isPathStale)
            {
                SetNextDestionation(false);
            }
        }
    }

    //Animiton rotation and position
    private void OnAnimatorMove()
    {
       // transform.rotation = animator.rootRotation;
        navAgent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    //Coroutine
    IEnumerator Jump(float duration)
    {
        //Acess to offlink mesh data
        OffMeshLinkData data = navAgent.currentOffMeshLinkData;
        Vector3 startPos = navAgent.transform.position;
        //Calculate position from transform and base offset of agent
        Vector3 endPos = data.endPos + (navAgent.baseOffset * Vector3.up);

        float time = 0.0f;

        while (time <= duration)
        {
            float t = time / duration;
            navAgent.transform.position = Vector3.Lerp(startPos, endPos, t) 
                + (JumpCurve.Evaluate(t) * Vector3.up);
            time += Time.deltaTime;
            yield return null;
        }

        //Agent will now know that he completed coruotine and will not stop
        navAgent.CompleteOffMeshLink();
    }

    //This function will make agent to waypoints 
    void SetNextDestionation (bool increment)
    {
        if (!WaypointNetwork)
        {
            return;
        }

        int incStep = increment ? 1 : 0;

        //If there is gap in the list we will step over it        
        int nextWaypoint = (CurrentIndex + incStep >=
            WaypointNetwork.waypoints.Count) ? 0 : CurrentIndex + incStep;

        Transform nextWaypointTransform = WaypointNetwork.waypoints[nextWaypoint];

        if (nextWaypointTransform != null)
        {
            CurrentIndex = nextWaypoint;
            navAgent.destination = nextWaypointTransform.position;
            return;
        }


        CurrentIndex++;
    }
}
