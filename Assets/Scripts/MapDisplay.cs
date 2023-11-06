using System;
using Map;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace GameplayLogic
{
    public class MapDisplay : MonoBehaviour
    {
        [SerializeField] MapLayout layoutScript;
        [SerializeField] Transform player;
        [SerializeField] GameObject markerParent;

        private Vector2 _playerPositionOnMap;
    
        private float _playerRotation;
        

        private bool _toggle;

        public bool Toggle => _toggle;

        [SerializeField] private RectTransform _playerIcon;
        [SerializeField] private RectTransform _currentDestination;
        [SerializeField] private RectTransform _mapRect;

        private TextMeshProUGUI _township;
    
        private BoxCollider _mapCollider;
    
        public static MapDisplay s_instance;

        void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            UpdateCurrentMap();
        }

        private void UpdateCurrentMap()
        {
            var col = layoutScript.MapCollider;
            if (col == null && _mapCollider == null)
            {
                _mapCollider = new BoxCollider();
            }
            _mapCollider = col;
        }
        
        public Vector2 GetPositionOnMapViaWorldPos(Vector3 pos)
        {
            return GetPositionOnMapRect(pos);
        }
        
        private Vector2 GetPositionOnMapRect(Vector3 worldPos)
        {
            if (_mapRect == null) return Vector2.zero;
            var bounds = MapHelper.GetBoxColliderCorners(_mapCollider);
            var coord =  MapHelper.GetMapPositionOfWorldObject(worldPos, bounds);
            return coord * _mapRect.rect.width;
        }

        private void ApplyPlayerPosition()
        {
            if(player != null) _playerIcon.anchoredPosition = GetPositionOnMapRect(player.transform.position);
        }

        private void ApplyPlayerRotation()
        {
            _playerIcon.rotation = Quaternion.Euler(0,0,-player.rotation.eulerAngles.y);
        }
    
        void Update()
        {
            if (!_mapRect.gameObject.activeSelf) return;
            ApplyPlayerPosition();
            ApplyPlayerRotation();
        }

        public void SetState(bool state)
        {
            if (_mapRect.gameObject.activeSelf != state) ToggleState();
        }

        public void ToggleState()
        {
            UpdateCurrentMap();

            var mapState = !_mapRect.gameObject.activeSelf;
            
            _mapRect.gameObject.SetActive(mapState);
            markerParent.SetActive(mapState);
        }


        public void TurnMapOff()
        {
            _mapRect.gameObject.SetActive(false);
        }

        public void SetDestination(Transform setDestination)
        {
            _currentDestination.anchoredPosition = GetPositionOnMapRect(setDestination.position);
            _currentDestination.gameObject.SetActive(true);
        }

        public void OnArriveAtDestination()
        {
            _currentDestination.gameObject.SetActive(false);
        }
    }
}