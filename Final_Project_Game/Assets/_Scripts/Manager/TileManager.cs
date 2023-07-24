using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NOOD;

namespace Game
{
    public class TileManager : MonoBehaviorInstance<TileManager>
    {
        [SerializeField] private Tilemap _interactableTileMap;
        private List<Vector3Int> _interactablePosition = new List<Vector3Int>();

        protected override void Awake() 
        {
            foreach(var tilePos in _interactableTileMap.cellBounds.allPositionsWithin)
            {
                _interactablePosition.Add(tilePos);
            }
            base.Awake();
            _interactableTileMap.gameObject.SetActive(false);
        }

        public bool IsInteractable(Vector3Int position)
        {
            if(_interactablePosition.Contains(position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
