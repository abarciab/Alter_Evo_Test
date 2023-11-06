using Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MinimapController : MonoBehaviour
{
    //REPLACE ON IMPLEMENTATION
    [HideInInspector] public Transform _player;
    [HideInInspector] public MapLayout _layoutScript;

    [Header("Parameters")]
    [SerializeField] float currentZoom;

    [Header("References")]
    [SerializeField] GameObject markerPrefab;
    [SerializeField] RectTransform mapBackground, mapPanParent, markerParent;
    
    RectTransform boundsRect;    
    Transform player;
    MapLayout layoutScript;
    Vector2 panLimits = new Vector2(1045, 1045);   
    List<KeyValuePair<MapMarker, RectTransform>> markers = new List<KeyValuePair<MapMarker, RectTransform>>();


    private void Start()
    {
        boundsRect = GetComponent<RectTransform>();

        //REPLACE ON IMPLEMENTATION
        player = _player;
        layoutScript = _layoutScript;
    }

    public void DisableCurrentDestination()
    {
        foreach (var m in markers) if (m.Key.enabled && m.Key.type == MapMarkerType.DisplayAlways) m.Key.enabled = false;
    }

    public void RegisterNewMarker(MapMarker markerData)
    {
        var newMarker = GenerateNewMarker(markerData.markerSprite, markerData.markerColor, markerData.type == MapMarkerType.DisplayWhenInRange);
        markers.Add(new KeyValuePair<MapMarker, RectTransform>(markerData, newMarker));
    }

    RectTransform GenerateNewMarker(Sprite sprite, Color color, bool maskable)
    {
        var newMarker = Instantiate(markerPrefab, markerParent);
        var img = newMarker.GetComponentInChildren<Image>();
        img.sprite = sprite; 
        img.color = color;
        img.maskable = maskable;
        newMarker.GetComponent<Canvas>().overrideSorting = !maskable;
        return newMarker.GetComponent<RectTransform>();
    }

    private void Update()
    {
        SetMapScale();

        if (!Application.isPlaying || player == null) return;

        mapBackground.transform.localEulerAngles = new Vector3(0, 0, player.transform.eulerAngles.y);
        mapPanParent.anchoredPosition = WorldToMapPos(player.position);
        foreach (var m in markers) DisplayMarker(m.Key, m.Value);
    }

    void DisplayMarker(MapMarker data, RectTransform marker)
    {
        marker.gameObject.SetActive(data.enabled);
        if (!data.enabled) return;

        marker.rotation = Quaternion.identity;
        var pos = WorldToMapPos(data.transform.position);
        marker.anchoredPosition = pos;

        if (data.type == MapMarkerType.DisplayAlways) ClampMarkerToEdgeOfMap(marker);
    }

    void ClampMarkerToEdgeOfMap(RectTransform marker)
    {
        var bounds = new Vector2(boundsRect.rect.width * boundsRect.lossyScale.x, boundsRect.rect.height * boundsRect.lossyScale.y) / 2;
        var worldPos = marker.position;
        var center = boundsRect.position;

        float xDist = worldPos.x - center.x;
        float yDist = worldPos.y - center.y;

        if (xDist >= bounds.x) worldPos.x = bounds.x + center.x;
        if (xDist <= -bounds.x) worldPos.x = -bounds.x + center.x;
        if (yDist >= bounds.y) worldPos.y = bounds.y + center.y;
        if (yDist <= -bounds.y) worldPos.y = -bounds.y + center.y;

        marker.position = worldPos;
    }

    void SetMapScale()
    {
        mapBackground.localScale = Vector3.one * currentZoom;
        foreach (var m in markers) m.Value.localScale = Vector3.one * m.Key.markerScale / currentZoom;
    }

    Vector2 WorldToMapPos(Vector3 worldPos)
    {
        var bounds = MapHelper.GetBoxColliderCorners(layoutScript.MapCollider);
        var normalizedPos = MapHelper.GetMapPositionOfWorldObject(worldPos, bounds);
        float x = (panLimits.x * 2 * normalizedPos.x) - panLimits.x;
        float y = (panLimits.y * 2 * normalizedPos.y) - panLimits.y;
        return new Vector2(x, y);
    }

    public void DeleteMarker(MapMarker markerData)
    {
        for (int i = 0; i < markers.Count; i++) {
            if (!markers[i].Key.Equals(markerData)) continue;

            if (markers[i].Value) Destroy(markers[i].Value.gameObject);
            markers.RemoveAt(i);
            return;
        }
    }
}
    
