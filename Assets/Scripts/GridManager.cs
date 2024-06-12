using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class in charge of generating the grid
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    int Width = 10;
    int Height = 10;
    
    public float CellSize = 1f;
    
    public IMap MapInterface;
    private IPathFinder PathFinderInterface;
    [SerializeField]
    private GameObject _cellPrefab;

    /// <summary>
    /// Set the singleton
    /// </summary>
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        MapInterface = new Map();
        PathFinderInterface = new PathFinder();
    }

    /// <summary>
    /// Assign the map and generate the grid
    /// </summary>
    private void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// Function to generate the grid this is generated using the grid width height and cellsize
    /// </summary>
    private void GenerateGrid()
    {
        // Generate hexagonal grid based on width, height, and cellSize
        float xOffset = 0f;
        float zOffset = 0f;
        
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                // Calculate position for the hexagon to create the hexagonal tiling
                float xPos = x * CellSize * Mathf.Sqrt(3f);
                float zPos = z * CellSize * 1.5f;

                // This condition adds a horizontal offset to every other row
                if (z % 2 != 0)
                {
                    xPos += CellSize * Mathf.Sqrt(3f) / 2f;
                }
                
                GameObject hexagon = Instantiate(_cellPrefab, new Vector3(xPos + xOffset, 0, zPos + zOffset), Quaternion.identity);
                hexagon.transform.SetParent(transform);

                // Set all the parameters of the cell
                Cell cell = hexagon.GetComponent<Cell>();
                cell.X = x;
                cell.Z = z;
                cell.Walkable = true;
                cell.Obstacle = cell.transform.GetChild(1).gameObject;
                MapInterface.AddCell(cell);
                hexagon.name = x + "," + z;
                hexagon.GetComponentInChildren<TextMeshPro>().text = x + "," + z;
            }
        }
    }

    /// <summary>
    /// Reset all the values of the cells to the original state also the UI elements
    /// </summary>
    public void ResetAll()
    {
        MapInterface.StartCell = null;
        MapInterface.EndCell = null;
        MapInterface.SetAllCellsWalkable();
        UIManager.Instance.ResetTextToOriginal();
    }

    /// <summary>
    /// Calls the find path getting the start and en cells defined by the user then just show it on the UI
    /// If there us no available path we display the no path found
    /// </summary>
    public void GetPath()
    {
        IList<ICell> path = PathFinderInterface.FindPathOnMap(MapInterface.StartCell, MapInterface.EndCell, MapInterface);
        
        if (path != null)
        {
            string coordinates = null;
            foreach (ICell cell in path)
            {
                coordinates =coordinates + " ("+cell.X+","+ cell.Z+")";
            }

            UIManager.Instance.CoordinatesText.text = coordinates;
        }
        else
        {
            UIManager.Instance.CoordinatesText.text = "No path found";
        }
    }
}