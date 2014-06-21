using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Material PenMaterial;

    private bool _isContinousDrawInProgress = false;
    private PathController _currentLetter;
    private GameManager _gameManager;
   
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

    void InitializePathController(RaycastHit2D hit)
    {
        if (_currentLetter != null) return; 

        _currentLetter = hit.transform.parent.GetComponent<PathController>();
        _currentLetter.PenMaterial = PenMaterial;
    }

    RaycastHit2D GetHighestPriorityHit(Vector3 worldpoint)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldpoint, Vector2.zero);
        var hit = new RaycastHit2D();
        
        if (hits.Length > 1)
        {
            // give priority to the next waypoint if hit
            foreach (var raycastHit2D in hits)
            {
                InitializePathController(raycastHit2D);
                if (raycastHit2D.collider != null && _currentLetter.IsCurrentWaypoint(raycastHit2D.transform.name))
                {
                    Debug.Log("Priority given to current waypoint.");
                    hit = raycastHit2D;
                    break;
                }
                hit = raycastHit2D;
            }
        }
        else
        {
            hit = hits.Length == 1 ? hits[0] : new RaycastHit2D();
            InitializePathController(hit);
        }
        return hit;
    }

    void Start()
    {
        _gameManager = this.gameObject.GetComponent<GameManager>();
        _gameManager.Next();
    }
	
	void FixedUpdate ()
	{
        //////******** FIRST CLICK/TOUCH  **********/////////////////
        if (IsUserFirstClickOrTouch() && !_isContinousDrawInProgress)
        {
            var worldpoint = InteractionWorldPoint();
            RaycastHit2D hit = GetHighestPriorityHit(worldpoint);
	        if (hit.collider != null)
	        {
                Debug.Log(hit.transform.name + ", " + _currentLetter.transform.name);
                if (_currentLetter.IsCurrentWaypoint(hit.transform.name))
                {
                    _isContinousDrawInProgress = true;
                    _currentLetter.AdvanceToNextWaypoint();
                    _currentLetter.CurrentWaypoint.DrawLineAt(new Vector3(worldpoint.x, worldpoint.y, 0));
                }
	        }
            return;
	    }

        //////******** CONTINUING A PREVIOUS CLICK/TOUCH  **********/////////////////
        if (IsUserContinuousClickOrTouch() && _isContinousDrawInProgress)
        {
            var worldpoint = InteractionWorldPoint();
            RaycastHit2D hit = GetHighestPriorityHit(worldpoint);

            // hitting the pathcollider object, all's good
            if (hit.collider != null && hit.transform.name == "PathCollider" && _currentLetter.CurrentWaypoint != null)
            {
                _currentLetter.CurrentWaypoint.DrawLineAt(new Vector3(worldpoint.x, worldpoint.y, 0));
            }
            // hit the next waypoint
            else if (hit.collider != null && _currentLetter.IsCurrentWaypoint(hit.transform.name))
            {
                Debug.Log("Hit the next waypoint: " + _currentLetter.CurrentWaypointIndex);
                _currentLetter.AdvanceToNextWaypoint();
                
                if (_currentLetter.CurrentWaypoint == null) //last waypoint
                {
                    Debug.Log("Reached the last waypoint.");
                    _currentLetter = null;
                    _isContinousDrawInProgress = false;
                    _gameManager.Next();
                    return;
                }
                
                if (_currentLetter.CurrentWaypoint.NoContinuousDraw)
                {
                    _isContinousDrawInProgress = false;
                }
                else
                {
                    _currentLetter.CurrentWaypoint.DrawLineAt(new Vector3(worldpoint.x, worldpoint.y, 0));
                }
            }
            else if (hit.collider == null)
            {
                Debug.Log("Failed to follow pathcollider. Stopping the continous draw.");
                _isContinousDrawInProgress = false;
                _currentLetter.GoBackToPreviousWaypoint();
            }
        }

        //////******** STOPPED A CONTINUOUS CLICK/TOUCH UNEXPECTEDLY  **********/////////////////
        if (!IsUserContinuousClickOrTouch() && _isContinousDrawInProgress)
        {
            Debug.Log("Stopped a continuous click/touch unexpectedly.");
            _isContinousDrawInProgress = false;
            _currentLetter.GoBackToPreviousWaypoint();
        }
	}
}
