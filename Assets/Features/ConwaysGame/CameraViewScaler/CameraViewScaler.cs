using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responds to changes in GridSize to scale our camera view accordingly
/// </summary>
public class CameraViewScaler : MonoBehaviour
{
    [field: SerializeField, Tooltip("Main camera ref. Will find on Awake() if left empty")]
    public Camera Cam { get; private set; } = null;

    [field: SerializeField, Tooltip("IntValue obj that controls the GridSize")]
    public IntValue GridSize { get; private set; } = null;

    private void UpdateCamera(int gridSize)
    {
        // Uses integer math to handle odd sizes
        Cam.orthographicSize = (gridSize + 1) / 2;

        // odd, shift camera, doesn't need to be optimized as this is rare operation
        if (gridSize % 2 != 0)
            Cam.transform.position = new Vector3(0.5f, 0.5f, Cam.transform.position.z);
        else
            Cam.transform.position = new Vector3(0f, 0f, Cam.transform.position.z);
    }

    private void Awake()
    {
        if (GridSize == null)
        {
            Debug.LogError("MISSING REF: CameraViewScaler is missing a ref to the GridSize variable");
            return;
        }

        if (Cam == null)
            Cam = Camera.main;

        UpdateCamera(GridSize.Value);
    }

    private void OnEnable()
    {
        if (!GridSize)
            return;

        GridSize.OnValueChanged += OnGridSizeChanged;
    }

    private void OnDisable()
    {
        if (!GridSize)
            return;

        GridSize.OnValueChanged -= OnGridSizeChanged;
    }

    /// <summary>
    /// Event handler for when the GridSize value changes during runtime.
    /// </summary>
    /// <param name="value">New grid size value</param>
    private void OnGridSizeChanged(dynamic value)
    {
        if (value is int)
        {
            UpdateCamera(value);
        }
    }
}
