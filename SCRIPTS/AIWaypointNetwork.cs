using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathDisplayMode
{
    None,
    Connections,
    Paths
}

public class AIWaypointNetwork : MonoBehaviour
{
    //Hiding this in Unity inspector
    [HideInInspector]
    public PathDisplayMode DisplayMode = PathDisplayMode.Connections;
    //For waypoints
    [HideInInspector]
    public int UIStart = 0;
    [HideInInspector]
    public int UIEnd = 0;



    public List<Transform> waypoints = new List<Transform>();

}
