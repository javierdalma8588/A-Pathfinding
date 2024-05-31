using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class in charge of handling the UI functionality
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI StartingPositionText;
    public TextMeshProUGUI EndPositionText;
    public TextMeshProUGUI CoordinatesText;

    public Button ResetButton;
    public Button CalculatePathButton;
    
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
    }

    /// <summary>
    /// Set the buttons listeners
    /// </summary>
    private void Start()
    {
        ResetButton.onClick.AddListener(GridManager.Instance.ResetAll);
        CalculatePathButton.onClick.AddListener(GridManager.Instance.GetPath);
    }

    /// <summary>
    /// Reset UI values to default
    /// </summary>
    public void ResetTextToOriginal()
    {
        EndPositionText.text = "X,X";
        StartingPositionText.text = "X,X";
        CoordinatesText.text = "";
        CalculatePathButton.interactable = true;
    }
}
