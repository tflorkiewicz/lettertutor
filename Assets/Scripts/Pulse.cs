using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

    public float RangePercentage = 10f;
    public float CycleTime = 1.0f;

    private int _sign = 1;
    private float _timer;
    private float _lastSwitch;
    private Vector3 _originalSize;

	// Use this for initialization
	void Start () {
        _originalSize = this.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        _timer = Time.time;
        if (_lastSwitch == null || _timer > _lastSwitch + (CycleTime / 2f))
        {
            LeanTween.scaleX(this.gameObject, _sign * 1 * _originalSize.x + (RangePercentage / 100f), CycleTime / 2.0f);
            LeanTween.scaleY(this.gameObject, _sign * 1 * _originalSize.y + (RangePercentage / 100f), CycleTime / 2.0f);
            _lastSwitch = Time.time;
            _sign *= -1;
        }
        
	}
}
