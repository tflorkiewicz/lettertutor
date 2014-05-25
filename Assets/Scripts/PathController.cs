using UnityEngine;

public class PathController : MonoBehaviour
{
    public int CurrentWaypointIndex = 1;
    public WaypointController CurrentWaypoint;
    public Material PenMaterial;
    
    public void GoBackToPreviousWaypoint()
    {
        if (CurrentWaypointIndex <= 1) return;

        var waypoint1 = transform.FindChild("Waypoint_" + (CurrentWaypointIndex-1));
        var waypoint2 = transform.FindChild("Waypoint_" + CurrentWaypointIndex);
        waypoint1.renderer.enabled = true;
        waypoint2.renderer.enabled = false;
        
        CurrentWaypoint.EraseLine();

        CurrentWaypoint = waypoint1.GetComponent<WaypointController>();
        CurrentWaypointIndex -= 1;
    }

    public void AdvanceToNextWaypoint()
    {
        if (CurrentWaypointIndex < 1) return;

        Debug.Log("Advancing to next waypoint.");

        var waypoint1 = transform.FindChild("Waypoint_" + CurrentWaypointIndex);
        var waypoint2 = transform.FindChild("Waypoint_" + (CurrentWaypointIndex + 1));

        //finish drawing the line , connecting to the center of the waypoint
        waypoint1.renderer.enabled = false;
        if (CurrentWaypoint != null) CurrentWaypoint.DrawLineAt(waypoint1.transform.position);

        if (waypoint2 == null)
        {
            Debug.Log("Letter complete!");
            CurrentWaypointIndex = 0;
            CurrentWaypoint = null;
            //LeanTween.move(this.gameObject, new Vector2(this.transform.position.x + 30, this.transform.position.y+30), 2);
            return;
        }

        waypoint2.renderer.enabled = true;

        // SCALING and PLACING THE COLLIDER
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
        pathCollider.transform.localScale = new Vector3(0.5f, hyp/4.0f, 1.0f);

        CurrentWaypointIndex += 1;
        CurrentWaypoint = waypoint2.GetComponent<WaypointController>();
        CurrentWaypoint.SetLineDetail(this.PenMaterial, 0.3f, 0.1f);
    }

    public bool IsCurrentWaypoint(string objectName)
    {
        return CurrentWaypointIndex > 0 && string.Equals("Waypoint_" + CurrentWaypointIndex, objectName);
    }
}

