using GameplayLogic;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapDisplay mapScript;
    MinimapController minimap;
    [SerializeField] MapMarker testDestination;

    [Space()]
    [SerializeField] Transform player;
    [SerializeField] MapLayout mapLayout;

    private void Awake()
    {
        mapScript.SetState(false);
        minimap = FindObjectOfType<MinimapController>(true);

        minimap._player = player;
        minimap._layoutScript = mapLayout;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) mapScript.ToggleState();
        if (Input.GetKeyDown(KeyCode.E)) ReachDestination();
        if (Input.GetKeyDown(KeyCode.R)) ActivateDestination();
    }

    public void ActivateDestination()
    {
        testDestination.enabled = true;
    }

    public void ReachDestination()
    {
        minimap.DisableCurrentDestination();
    }
}
