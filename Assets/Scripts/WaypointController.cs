using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WaypointController : MonoBehaviour
{
    public bool NoContinuousDraw;
  
    private LineRenderer _line;
    private List<Vector3> _lineVertices;

    void Start()
    {
        InitializeLineRenderer();
    }

    void InitializeLineRenderer()
    {
        _lineVertices = new List<Vector3>();
        _line = this.gameObject.AddComponent<LineRenderer>();
        _line.useWorldSpace = false;
        _line.SetColors(new Color(0f, 125f, 0f), new Color(0f, 125f, 0f));
    }

    public void SetLineDetail(Material material, float widthStart, float widthEnd)
    {
        this._line.material = material;
        this._line.SetWidth(widthStart, widthEnd);
    }
    
    public void DrawLineAt(Vector3 worldpoint)
    {
        _lineVertices.Add(new Vector3(worldpoint.x - this.transform.position.x, worldpoint.y - this.transform.position.y, 0));
        _line.SetVertexCount(_lineVertices.Count);
        _line.SetPosition(_lineVertices.Count - 1, _lineVertices[_lineVertices.Count - 1]);
    }

    public void EraseLine()
    {
        _lineVertices.Clear();
        _line.SetVertexCount(0);
    }
}
