using System;
using UnityEngine;

namespace MultiplayerGame.Code.Core.UI.Rooms.CreateRoom
{
    public class MapSelectPanel : MonoBehaviour
    {
        public event Action<int> OnMapSelected;
        public Transform Content;
        [HideInInspector] public int SelectedMapId = -1;
        private MapSelectElement[] _mapSelectElements;

        public void Construct(MapSelectElement[] mapSelectElements)
        {
            _mapSelectElements = mapSelectElements;
            foreach (MapSelectElement selectElement in _mapSelectElements) 
                selectElement.OnSelected += SelectMap;
        }

        private void SelectMap(int mapId)
        {
            if(SelectedMapId != -1) _mapSelectElements[SelectedMapId].SetUnselectedStateView();
            SelectedMapId = mapId;
            _mapSelectElements[SelectedMapId].SetSelectedStateView();
            OnMapSelected?.Invoke(mapId);
        }
    }
}