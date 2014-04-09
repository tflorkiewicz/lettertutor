using UnityEngine;
using System.Collections;

public class PathController : MonoBehaviour
{
    public int CurrentWaypoint = 1;

    // Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update ()
	{
    }

    public void Advance()
    {
        Debug.Log("Advancing to next waypoint.");
        var waypoint1 = transform.FindChild("Waypoint_" + CurrentWaypoint);
        var waypoint2 = transform.FindChild("Waypoint_" + (CurrentWaypoint + 1));
        waypoint1.renderer.enabled = false;
        waypoint2.renderer.enabled = true;

        var pathCollider = transform.FindChild("PathCollider");

        // calculating the distance between the two waypoints
        var deltaX = waypoint2.transform.position.x - waypoint1.transform.position.x;
        var deltaY = waypoint2.transform.position.y - waypoint1.transform.position.y;

        // calculating the midpoint of the two waypoints
        var midX = waypoint1.transform.position.x + 0.5f*deltaX;
        var midY = waypoint1.transform.position.y + 0.5f*deltaY;
        
        // placing the pathcollider centered at that midpoint
        pathCollider.transform.position = new Vector3(midX, midY);

        // rotate pathcollider toward the next waypoint
        pathCollider.forward = Vector3.up;
        var deltaVector = waypoint2.transform.position - waypoint1.transform.position;
        var quat = Quaternion.FromToRotation(pathCollider.forward, deltaVector);
        pathCollider.transform.rotation = quat;

        //scaling the pathcollider
        var hyp = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        pathCollider.transform.localScale = new Vector3(1, hyp/2.0f, 1.0f);

        CurrentWaypoint += 1;

    }
}

