using UnityEngine;

namespace Tarodev_Pathfinding._Scripts.Units {
    public class Unit2 : MonoBehaviour {
        [SerializeField] private SpriteRenderer _renderer;

        public void Init(Sprite sprite) {
            _renderer.sprite = sprite;
        }
    }
}
