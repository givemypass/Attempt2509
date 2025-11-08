using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.GameScreenFeature.Mono
{
    public class ColorSignMonoComponent : MonoBehaviour
    {
        [SerializeField] private Image _signImage;
        
        private Tween _tween;
        
        public void SetColor(Color color)
        {
            _signImage.color = color;
        }

        public void PlayHideState()
        {
            _tween?.Rewind();
            _tween?.Kill();
            _tween = transform.DOScale(0,0.3f).SetEase(Ease.InBack).SetLink(gameObject);
        }
        
        public void PlayShowState()
        {
            _tween?.Rewind();
            _tween?.Kill();
            var scale = transform.localScale;
            transform.localScale = Vector3.zero;
            _tween = transform.DOScale(scale,0.3f).SetLink(gameObject);
        }
        
        public void PlaySelectedState()
        {
            _tween?.Rewind();
            _tween?.Kill();
            transform.DOPunchScale(transform.localScale * 0.5f, 0.25f).SetEase(Ease.OutElastic).SetLink(gameObject);
        }

        public void PlayHintState()
        {
            _tween?.Rewind();
            _tween?.Kill();
            var sequence = DOTween.Sequence();
            sequence
                .Join(transform
                    .DOPunchScale(transform.localScale * 0.3f, 0.5f, 1, 0).SetEase(Ease.InOutBack)
                    .SetLoops(-1, LoopType.Restart))
                .Join(_signImage
                    .DOColor(Color.white, 0.25f)
                    .SetLoops(-1, LoopType.Yoyo))
                .SetLink(gameObject);
            _tween = sequence;
        }
        
        public void RewindState()
        {
            _tween?.Rewind();
            _tween?.Kill();
            _tween = null;
        }
    }
}