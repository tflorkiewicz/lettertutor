using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    private bool _isDrawing = false;
	
	// Update is called once per frame
	void Update ()
	{
	    if (Application.isEditor)
	    {
	        if (Input.GetMouseButtonDown(0) && !_isDrawing)
	        {
	            Debug.Log(Input.mousePosition);
	            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                Debug.Log(hit);
	            if (hit.collider != null)
	            {
	                Debug.Log("Hit object: " + hit.transform.name);
	                var pathController = hit.transform.parent.GetComponent<PathController>();
	                if (pathController == null)
	                {
	                    Debug.Log("Cannot locate PathController");
	                    return;
	                }

                    if (string.Equals(hit.transform.name, "Waypoint_" + pathController.CurrentWaypoint))
                    {
                        _isDrawing = true;
                        pathController.Advance();
                    }
	            }
	        }
            if (Input.GetMouseButton(0) && _isDrawing)
            {
                Debug.Log("pressing mouse button");
            }
	        return;
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
