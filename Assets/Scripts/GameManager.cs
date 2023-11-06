using GameplayLogic;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapDisplay mapScript;
    [SerializeField] MinimapController minimap;
    [SerializeField] MapMarker testDestination;

    [Space()]
    [SerializeField] Transform player;
    [SerializeField] MapLayout mapLayout;

    private void Awake()
    {
        Init();
    }

    void Init() {
        minimap = FindObjectOfType<MinimapController>(true);
        if (!minimap) return;

        mapScript.SetState(false);

        minimap._player = player;
        minimap._layoutScript = mapLayout;
    }

    private void Update()
    {
        if (!minimap) {
            Init();
            return;
        }

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
