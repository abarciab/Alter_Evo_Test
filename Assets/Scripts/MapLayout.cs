using UnityEngine;

namespace Map
{
    public class MapLayout : MonoBehaviour
    {
        [SerializeField] BoxCollider _mapCollider;
        public BoxCollider MapCollider => _mapCollider;
    }
}