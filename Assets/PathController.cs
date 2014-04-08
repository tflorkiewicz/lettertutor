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
        var waypoint1 = transform.FindChild("Waypoint_" + CurrentWaypoint);
        var waypoint2 = transform.FindChild("Waypoint_" + (CurrentWaypoint + 1));

        var pathCollider = transform.FindChild("PathCollider");

        var deltaX = waypoint2.transform.position.x - waypoint1.transform.position.x;
        var deltaY = waypoint2.transform.position.y - waypoint1.transform.position.y;

        var hyp = Mathf.Sqrt(deltaX*deltaX + deltaY*deltaY);
        var midX = waypoint1.transform.position.x + 0.5f*deltaX;
        var midY = waypoint1.transform.position.y + 0.5f*deltaY;

        pathCollider.transform.position = new Vector3(midX, midY);
        pathCollider.transform.localScale = new Vector3(1, hyp);

        //Vector3 temp = pathCollider.transform.localEulerAngles;
        pathCollider.transform.LookAt(waypoint2.transform.position);
        //temp.y = pathCollider.transform.localEulerAngles.y;
        //pathCollider.transform.localEulerAngles = temp;

        CurrentWaypoint += 1;
    }
}

