using System.Collections;
using Game.Code.Core.UI.Base;
using UnityEngine;

namespace Game.Code.Services.LoadingCurtain
{
    public class LoadingCurtain : BaseWindow, ILoadingCurtain
    {
        [SerializeField] private CanvasGroup _curtain;

        public override void Show()
        {
            gameObject.SetActive(true);
            _curtain.alpha = 1;
        }
        
        public override void Hide()
        {
            if(gameObject.activeSelf) StartCoroutine(DoFadeIn());
        }

        private IEnumerator DoFadeIn()
        {
            while (_curtain.alpha > 0)
            {
                _curtain.alpha -= Time.deltaTime * 3;
                yield return null;
            }
      
            gameObject.SetActive(false);
        }
    }
}