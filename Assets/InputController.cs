using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour 
{
    public GameObject Pen;
    public float DrawPause;

    private bool _isDrawing = false;
    private float _lastDrawTime;
    private PathController _pathController;

    void Start()
    {
        _lastDrawTime = Time.time;
    }

    private bool IsUserFirstClickOrTouch()
    {
        //todo: implement for touches
        return Application.isEditor ? Input.GetMouseButton(0) : false;
    }
    private bool IsUserContinuousClickOrTouch()
    {
        //todo: implement for touches
        return Application.isEditor ? Input.GetMouseButton(0) : false;
    }

    private Vector3 InteractionWorldPoint()
    {
        //todo: implement for touches
        return Application.isEditor ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Vector3.zero;
    }
	
	void Update ()
	{
        if (IsUserFirstClickOrTouch() && !_isDrawing)
	    {
            RaycastHit2D hit = Physics2D.Raycast(InteractionWorldPoint(), Vector2.zero);
	        if (hit.collider != null)
	        {
	            _pathController = hit.transform.parent.GetComponent<PathController>();
                if (_pathController == null)
	            {
	                Debug.Log("Fatal error: Cannot locate PathController");
	                return;
	            }
                // If user clicked the 'next' waypoint
                if (string.Equals(hit.transform.name, "Waypoint_" + _pathController.CurrentWaypoint))
                {
                    _isDrawing = true;
                    _pathController.Advance();
                }
	        }
            return;
	    }

        if (IsUserContinuousClickOrTouch() && _isDrawing)
        {
            RaycastHit2D hit = Physics2D.Raycast(InteractionWorldPoint(), Vector2.zero);
                
            // drawing using the pen field if inside the pathcollider object
            if (hit.collider != null && hit.transform.name == "PathCollider")
            {
                if (Time.time > _lastDrawTime + DrawPause)
                {
                    var mark = Instantiate(Pen, hit.point, Quaternion.identity) as GameObject;
                    _pathController.AddPenMark(mark.transform);
                    _lastDrawTime = Time.time;
                }
            }
            // hit the next waypoint!
            else if (hit.collider != null && hit.transform.name.StartsWith("Waypoint_" + _pathController.CurrentWaypoint))
            {
                _pathController.Advance();
            }
        }
        
        if (!IsUserContinuousClickOrTouch() && _isDrawing)
        {
            Debug.Log("Stopped drawing :(");
            _isDrawing = false;
            _pathController.Retreat();
        }
	}
}
