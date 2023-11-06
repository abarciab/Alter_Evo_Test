using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapMarkerType { DisplayWhenInRange, DisplayAlways };

public class MapMarker: MonoBehaviour
{
    MinimapController controller;

    [SerializeField] float _markerScale = 1;
    [SerializeField] Sprite _markerSprite;
    [SerializeField] Color _markerColor = Color.white;
    [SerializeField] MapMarkerType _type;

    public MapMarkerType type => _type;
    public float markerScale => _markerScale;
    public Sprite markerSprite => _markerSprite;
    public Color markerColor => _markerColor;

    void Start()
    {
        controller = FindObjectOfType<MinimapController>(true);
        controller.RegisterNewMarker(this);
    }

    private void OnDestroy()
    {
        if (controller) controller.DeleteMarker(this);
    }
}
