using UnityEngine;

/// <summary>
/// Interface for the cells (Add all the cell variables here )
/// </summary>
public interface ICell
{
    int X { get; }
    int Z { get; }
    bool Walkable { get; set; }
    GameObject Obstacle { get;}
}