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

        if ((!HasPath && !PathPending) ||
            PathStatus == NavMeshPathStatus.PathInvalid)
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
            Transform nextWaypointTransform = null;

            //If there is gap in the list we will step over it
            while (nextWaypointTransform == null)
            {
                int nextWaypoint = (CurrentIndex + incStep >= 
                    WaypointNetwork.waypoints.Count) ? 0 : CurrentIndex + incStep;

                nextWaypointTransform = WaypointNetwork.waypoints[nextWaypoint];

                if (nextWaypointTransform != null)
                {
                    CurrentIndex = nextWaypoint;
                    navAgent.destination = nextWaypointTransform.position;
                    return;
                }
            }

            CurrentIndex++;
    }
}
