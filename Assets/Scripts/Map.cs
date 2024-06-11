using System.Collections.Generic;

/// <summary>
/// Class inheriting from the IMap interface in charge of handling the map variables
/// </summary>
public class Map : IMap
{
    private Dictionary<(int, int), ICell> _cells = new Dictionary<(int, int), ICell>();
    //Reusable list
    private List<ICell> _reusableNeighborsList = new List<ICell>();

    public Cell StartCell { get; set; }
    public Cell EndCell { get; set; }

    /// <summary>
    /// Adds the generated cell with its coordinates
    /// </summary>
    /// <param name="cell"></param>
    public void AddCell(Cell cell)
    {
        _cells[(cell.X, cell.Z)] = cell;
    }

    /// <summary>
    /// Get a certain cell depending on the coordinates we try to get the value to make sure it exists in the cells dictionary
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private ICell GetCell(int x, int z)
    {
        _cells.TryGetValue((x, z), out ICell cell);
        return cell;
    }

    /// <summary>
    /// Function to reset all of the cells to ist original value (walkable)
    /// </summary>
    public void SetAllCellsWalkable()
    {
        foreach (var cell in _cells.Values)
        {
            cell.Walkable = true;
            cell.Obstacle.SetActive(false);
        }
    }

    /// <summary>
    /// Get neighbors from a specific cell coordinates I had to create a offset for even and odd coordinates otherwise I was getting a wrong coordinate for those edge cases
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public IEnumerable<ICell> GetNeighbors(ICell cell)
    {
        // clear the list so we can call this function instead of creating a new one every time
        _reusableNeighborsList.Clear();

        // Define the neighbor offsets based on whether the row (Z coordinate) is even or odd
        int[][] evenRowOffsets = new int[][]
        {
            new int[] { +1, 0 },
            new int[] { 0, -1 },
            new int[] { -1, -1 },
            new int[] { -1, 0 },
            new int[] { -1, +1 },
            new int[] { 0, +1 }
        };

        int[][] oddRowOffsets = new int[][]
        {
            new int[] { +1, 0 },
            new int[] { +1, -1 },
            new int[] { 0, -1 },
            new int[] { -1, 0 },
            new int[] { 0, +1 },
            new int[] { +1, +1 }
        };

        // Check if the row is odd or even depending on the z coordinate
        int[][] offsets = (cell.Z % 2 == 0) ? evenRowOffsets : oddRowOffsets;

        foreach (var offset in offsets)
        {
            int neighborX = cell.X + offset[0];
            int neighborZ = cell.Z + offset[1];

            ICell neighbor = GetCell(neighborX, neighborZ);
            // Make sure that the neighbor is walkable
            if (neighbor != null && neighbor.Walkable)
            {
                _reusableNeighborsList.Add(neighbor);
            }
        }

        return _reusableNeighborsList;
    }
}
