using UnityEngine;
using System.Collections;

public class PathController : MonoBehaviour
{
    public int CurrentWaypoint = 1;
    public ParticleSystem WaypointParticles = null;
    private ParticleSystem _activeParticleSystem = null;

    public void AddPenMark(Transform penMark)
    {
        // adding a pen mark as a child of the previous waypoint (to make things easier to remove the pen marks later)
        var waypoint = transform.FindChild("Waypoint_" + (CurrentWaypoint-1));
        penMark.parent = waypoint;        
    }

    public void Retreat()
    {
        if (CurrentWaypoint <= 1) return;

        var waypoint1 = transform.FindChild("Waypoint_" + (CurrentWaypoint-1));
        var waypoint2 = transform.FindChild("Waypoint_" + CurrentWaypoint);
        waypoint1.renderer.enabled = true;
        waypoint2.renderer.enabled = false;

        // delete all pen marks
        int children = waypoint1.childCount;
        for (int i = 0; i < children; i++)
        {
            DestroyImmediate(waypoint1.GetChild(0).gameObject);
        }

        CurrentWaypoint -= 1;
    }

    public void Advance()
    {
        Debug.Log("Advancing to next waypoint.");
        if (_activeParticleSystem != null)
        {
            _activeParticleSystem.Stop();
            Destroy(_activeParticleSystem);
        }

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

        _activeParticleSystem = Instantiate(WaypointParticles, waypoint2.transform.position, Quaternion.identity) as ParticleSystem;
        _activeParticleSystem.Play();        
    }
}

