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
        [SerializeField] private Tilemap _interactableTileMap2;
        private List<Vector3Int> _interactablePosition = new List<Vector3Int>();
        private List<Vector3Int> _interactablePosition2 = new List<Vector3Int>();
        private GameObject _HightLight;
        protected override void Awake() 
        {
            foreach(var tilePos in _interactableTileMap.cellBounds.allPositionsWithin)
            {
                _interactablePosition.Add(tilePos);
            }
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

        public void InteractableHere(Vector3Int position)
        {
            if (!_interactablePosition2.Contains(position))
            {
                _interactablePosition2.Add(position);
            }
            else
            {
                Debug.Log("đã có");
            }
        }

        public void AllPos()
        {
            for (int i = 0; i < _interactablePosition2.Count; i++)
            {
                Debug.Log("____position______" + _interactablePosition2[i]);
            }
        }

    }

}
