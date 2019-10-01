using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour //Attaches to a waypoint gameobject and hold all the information it uses
{
    [SerializeField]
    protected float _connectivityRadius = 50f;

    public List<Waypoint> _connections;
    public bool enableWireframe;

    public void Start()
    {
        //Grab all waypoint objects in scene.
        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //Create a list of waypoints I can refer to later.
        _connections = new List<Waypoint>();

        //Check if they're a connected waypoint.
        for (int i = 0; i < allWaypoints.Length; i++)
        {
            Waypoint nextWaypoint = allWaypoints[i].GetComponent<Waypoint>();

            //i.e. we found a waypoint.
            if (nextWaypoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= _connectivityRadius && nextWaypoint != this)
                {
                    _connections.Add(nextWaypoint);
                }
            }
        }
    }

    public void OnDrawGizmos() //when a bool is enabled, draw wireframes to show the range and position of the waypoints
    {
        if(enableWireframe)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
        }
    }

    public Waypoint NextWaypoint(Waypoint previousWaypoint) //called from the search state of bots to generate a new waypoint
    {
        if (_connections.Count == 0)
        {
            //No waypoints?  Return null and complain.
            Debug.LogError("Insufficient waypoint count.");
            return null;
        }
        else if (_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            //Only one waypoint and it's the previous one? Just use that.
            Debug.Log("No other waypoints found to go to, returning to previous point");
            return previousWaypoint;
        }
        else //Otherwise, find a random one that isn't the previous one.
        {
            Waypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = Random.Range(0, _connections.Count);
                nextWaypoint = _connections[nextIndex];

            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
