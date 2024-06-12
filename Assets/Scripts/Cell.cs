using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class inheriting from the ICell interface used mainly to handle the cell behaviour
/// </summary>
public class Cell : MonoBehaviour, ICell
{
    private Map _map;
    
    [Header("Color change")]
    [SerializeField]
    private Color mouseOverColor;
    private Color _originalColor;
    private MeshRenderer _renderer;

    public int X { get; set; }
    public int Z { get; set; }
    public bool Walkable { get; set; } 
    public GameObject Obstacle { get; set; }
    public Cell(int x, int z, bool walkable = true)
    {
        X = x;
        Z = z;
        Walkable = walkable;
    }

    /// <summary>
    /// Get components to fill the variables
    /// </summary>
    private void Start()
    {
        _map = (Map) GridManager.Instance.MapInterface;
        _renderer = GetComponent<MeshRenderer>();
        _originalColor = _renderer.material.color;
    }

    /// <summary>
    /// Just a debug function for testing that I am getting the actual neighbors
    /// </summary>
    [ContextMenu("Print Neighbors")]
    private void PrintNeighbors()
    {
        IEnumerable<ICell> neighbors = _map.GetNeighbors(this);
        
        foreach (ICell neighbor in neighbors)
        {
            Debug.Log("Neighbor: " + neighbor.X + ", " + neighbor.Z);
        }
    }
    
    /// <summary>
    /// Actions on being on top of the cell depending on the button you clicked
    /// </summary>
    private void OnMouseOver()
    {
        _renderer.material.color = mouseOverColor;
        if (Input.GetMouseButtonDown(0))
        {
            Walkable = !Walkable;
            Obstacle.SetActive(!Obstacle.activeInHierarchy);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Walkable)
            {
                _map.StartCell = this;
                UIManager.Instance.StartingPositionText.text = X + "," + Z;

                if (_map.StartCell != null && _map.EndCell != null)
                {
                    UIManager.Instance.CalculatePathButton.interactable = true;
                }
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (Walkable)
            {
                _map.EndCell = this;
                UIManager.Instance.EndPositionText.text = X + "," + Z;
                if (_map.StartCell != null && _map.EndCell != null)
                {
                    UIManager.Instance.CalculatePathButton.interactable = true;
                }
            }
        }
    }
    
    /// <summary>
    /// On exiting the mouse we go back to the original color
    /// </summary>
    private void OnMouseExit()
    {
        _renderer.material.color = _originalColor;
    }
}


