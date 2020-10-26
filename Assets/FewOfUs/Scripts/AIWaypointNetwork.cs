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
    public PathDisplayMode DisplayMode = PathDisplayMode.Connections;



    public List<Transform> waypoints = new List<Transform>();

}
