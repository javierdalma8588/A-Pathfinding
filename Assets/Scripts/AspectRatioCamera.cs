using UnityEngine;

/// <summary>
/// Class to handle the camera aspect ratio depending the ratio adjust the camera aspect
/// </summary>
[RequireComponent(typeof(Camera))][ExecuteAlways]
public class AspectRatioCamera : MonoBehaviour
{
    [SerializeField] 
    private Camera _cam;
   
    //I used the iphone 12 as the base aspect ratio
    private readonly Vector2 _targetAspectRatio = new(19.5f,9);
    private readonly Vector2 _rectCenter = new(0.5f, 0.5f);
    private Vector2 _lastResolution;
 
    /// <summary>
    /// Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    /// </summary>
    private void OnValidate()
    {
        _cam ??= GetComponent<Camera>();
    }
 
    /// <summary>
    /// calculate depending on the screen aspect ratio
    /// </summary>
    public void LateUpdate()
    {
        var currentScreenResolution = new Vector2(Screen.width, Screen.height);
 
        // Don't run all the calculations if the screen resolution has not changed
        if (_lastResolution != currentScreenResolution)
        {
            CalculateCameraRect(currentScreenResolution);
        }
 
        _lastResolution = currentScreenResolution;
    }
 
    /// <summary>
    /// Calculate the camera aspect ratio
    /// </summary>
    /// <param name="currentScreenResolution"></param>
    private void CalculateCameraRect(Vector2 currentScreenResolution)
    {
        var normalizedAspectRatio = _targetAspectRatio / currentScreenResolution;
        var size = normalizedAspectRatio / Mathf.Max(normalizedAspectRatio.x, normalizedAspectRatio.y);
        _cam.rect = new Rect(default, size) { center = _rectCenter };
    }
}
