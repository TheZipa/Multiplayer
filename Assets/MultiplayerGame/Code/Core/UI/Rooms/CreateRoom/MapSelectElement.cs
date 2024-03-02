using System;
using MultiplayerGame.Code.Data.StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms.CreateRoom
{
    public class MapSelectElement : MonoBehaviour
    {
        public event Action<int> OnSelected;
        [SerializeField] private Button _selectButton;
        [SerializeField] private Image _mapPreview;
        [SerializeField] private TextMeshProUGUI _mapName;
        [Space(10)]
        [SerializeField] private Color _selectedColor;
        private Color _unselectedColor;
        private int _id;

        private void Awake()
        {
            _selectButton.onClick.AddListener(() => OnSelected?.Invoke(_id));
            _unselectedColor = _selectButton.image.color;
        }

        public void Construct(MapData mapData, int id)
        {
            _mapPreview.sprite = mapData.MapPreview;
            _mapName.text = mapData.Name;
            _id = id;
        }

        public void SetSelectedStateView() => _selectButton.image.color = _selectedColor;

        public void SetUnselectedStateView() => _selectButton.image.color = _unselectedColor;
    }
}