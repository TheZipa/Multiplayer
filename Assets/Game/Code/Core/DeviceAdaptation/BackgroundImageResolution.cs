using Game.Code.Data.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.Core.DeviceAdaptation
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class BackgroundImageResolution : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Sprite _iPhoneBackground, _iPadBackground;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private AspectRatioFitter _aspectRatioFitter;
        [SerializeField] private Vector2 _sliceOffset;
        private BackgroundOrientation _orientation;

        private const int iPadHeight = 1500;
        private const float iPhonePortraitAspectRatio = 0.46f;
        private const float iPadPortraitAspectRatio = 0.75f;
        private const float iPadLandscapeAspectRatio = 1.2f;
        private const float iPhoneLandscapeAspectRatio = 2.2f;
        private const float iPhoneXRRatio = 1.87f;

        private void Start() => AdjustResolution();

        public void Configure(Sprite background_iPhone, Sprite background_iPad)
        {
            _iPhoneBackground = background_iPhone;
            _iPadBackground = background_iPad;
            AdjustResolution();
        }

        private void AdjustResolution()
        {
            SetBackgroundOrientation();
            bool isiPad = GetCurrentDeviceHeight() >= iPadHeight;
            SetImageResolution(isiPad);
            SetSlice(isiPad);
            _rectTransform.sizeDelta = Vector2.zero;
        }

        private void SetBackgroundOrientation() => _orientation = Screen.height > Screen.width
            ? BackgroundOrientation.Portrait
            : BackgroundOrientation.Landscape;

        private int GetCurrentDeviceHeight() => _orientation == BackgroundOrientation.Landscape ? Screen.height : Screen.width;

        private void SetImageResolution(bool isiPad) => _backgroundImage.sprite = isiPad ? _iPadBackground : _iPhoneBackground;

        private void SetSlice(bool isiPad)
        {
            ConfigureAspectRatioFitter(isiPad);
            ApplyOffset(isiPad);
        }

        private void ApplyOffset(bool isiPad) =>
            _rectTransform.anchoredPosition = isiPad || GetAspectRatio() >= iPhoneXRRatio 
                ? Vector2.zero : new Vector2(_sliceOffset.x, _sliceOffset.y);

        private void ConfigureAspectRatioFitter(bool isiPad)
        {
            if (_orientation == BackgroundOrientation.Landscape) SetAspectRatioByLandscape(isiPad);
            else SetAspectRatioByPortrait(isiPad);
        }

        private void SetAspectRatioByLandscape(bool isiPad)
        {
            _aspectRatioFitter.aspectMode = isiPad
                ? AspectRatioFitter.AspectMode.WidthControlsHeight
                : AspectRatioFitter.AspectMode.HeightControlsWidth;
            _aspectRatioFitter.aspectRatio = isiPad ? iPadLandscapeAspectRatio : iPhoneLandscapeAspectRatio;
        }

        private void SetAspectRatioByPortrait(bool isiPad)
        {
            _aspectRatioFitter.aspectMode = isiPad
                ? AspectRatioFitter.AspectMode.HeightControlsWidth
                : AspectRatioFitter.AspectMode.WidthControlsHeight;
            _aspectRatioFitter.aspectRatio = isiPad ? iPadPortraitAspectRatio : iPhonePortraitAspectRatio;
        }

        private float GetAspectRatio()
        {
            Vector2Int screenSize = _orientation == BackgroundOrientation.Landscape
                ? new Vector2Int(Screen.width, Screen.height)
                : new Vector2Int(Screen.height, Screen.width);
            return screenSize.x / screenSize.y;
        }
    }
}
