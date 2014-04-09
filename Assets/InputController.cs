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
	
	void Update ()
	{
	    if (Application.isEditor)
	    {
	        if (Input.GetMouseButtonDown(0) && !_isDrawing)
	        {
	            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
	            if (hit.collider != null)
	            {
	                _pathController = hit.transform.parent.GetComponent<PathController>();
                    if (_pathController == null)
	                {
	                    Debug.Log("Cannot locate PathController");
	                    return;
	                }
                    // If user clicked the 'next' waypoint
                    if (string.Equals(hit.transform.name, "Waypoint_" + _pathController.CurrentWaypoint))
                    {
                        _isDrawing = true;
                        _pathController.Advance();
                    }
	            }
	        }

            if (Input.GetMouseButton(0) && _isDrawing)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                
                // drawing using the pen field
                if (hit.collider != null && hit.transform.name == "PathCollider")
                {
                    if (Time.time > _lastDrawTime + DrawPause)
                    {
                        Instantiate(Pen, hit.point, Quaternion.identity);
                        _lastDrawTime = Time.time;
                    }
                    //todo: stagger instantiating by time
                }
                // hit the next waypoint!
                else if (hit.collider != null && hit.transform.name.StartsWith("Waypoint_" + _pathController.CurrentWaypoint))
                {
                    _pathController.Advance();
                }
                else
                {
                    Debug.Log("Hit something else!!!");
                }
            }
            else if (!Input.GetMouseButton(0) && _isDrawing)
            {
                Debug.Log("Stopped drawing :(");
                _isDrawing = false;
                //todo: tell pathcontroller to reset back to previous waypoint
            }

            return; // exit unity editor input controller
	    }

	    var touches = Input.touches;
	    foreach (var touch in touches)
	    {
           Debug.Log(touch.position);
	       if (touch.phase == TouchPhase.Began)
	       {
               Debug.Log("Began touch");
	           RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

               if (hit.collider != null)
               {
                   Debug.Log("Hit object: " + hit.transform.name);
               }
	       }
	    }
	}
}
