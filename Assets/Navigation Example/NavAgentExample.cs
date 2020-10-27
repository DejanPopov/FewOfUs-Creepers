using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentExample : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        //NavMesh agent reference
        navAgent = GetComponent<NavMeshAgent>();

        //This code is to see for fun that if the animated enemy is not moving
        //but NavMeshAgent is moving then this is probably whats happend
        /*
        navAgent.updatePosition = false;
        navAgent.updateRotation = false;
        */

        if (WaypointNetwork == null)
        {
            return;
        }

        SetNextDestionation(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Will show in inspector
        HasPath = navAgent.hasPath;
        PathPending = navAgent.pathPending;
        PathStale = navAgent.isPathStale;
        PathStatus = navAgent.pathStatus;

        //This will be used for agent to start coroutine called JUMP
        if (navAgent.isOnOffMeshLink)
        {
            StartCoroutine(Jump(2.0f));
            return;
        }

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
            navAgent.transform.position = Vector3.Lerp(startPos, endPos, t);
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
