using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

/// <summary>
/// This class is used to handle the user manually toggling tile states by clicking on them
/// </summary>
public class ClickDetector : MonoBehaviour, IPointerDownHandler
{
    [field: SerializeField]
    public Tilemap Tilemap { get; private set; } = null;
    [field: SerializeField]
    public Vector3IntEvent TileClickedEvent { get; private set; } = null;

    public void Awake()
    {
        if (!Tilemap || !TileClickedEvent)
        {
            Debug.LogError("ClickDetector is missing references and the game will not work properly!");
            this.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 worldPosition = eventData.pointerCurrentRaycast.worldPosition;
        Vector3Int tileCoord = Tilemap.WorldToCell(worldPosition);

        TileClickedEvent.Raise(tileCoord);
    }
}
