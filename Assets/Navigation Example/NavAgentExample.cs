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

        //!HasPath will be little buggy to use when agent is jumping across platforms
        //We can use this || PathStatus == NavMeshPathStatus.PathPartial into
        //if () statement and then the agent will not go to invalid path that cant reach
        if ((/*!HasPath*/ navAgent.remainingDistance.Equals(0.0f) && !PathPending) ||
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
