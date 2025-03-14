using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private float bounceStrength = 1.5f;
        [SerializeField] private float bounceDuration = 0.1f;
        [SerializeField] private Ease bounceEase = Ease.InSine;
        
        private Selectable _selectable;
        private Tween _currentTween;
        private Vector3 _originalScale;
        
        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _originalScale = transform.localScale;
        }

        public void Highlight(bool highlight = true)
        {
            if (highlight)
            {
                // If there is an actual button: Show the highlight colour
                _selectable?.OnPointerEnter(null);
                
                if (_currentTween != null && _currentTween.IsActive() && _currentTween.IsPlaying())
                {
                    return;
                }
                
                _currentTween = transform.DOScale(bounceStrength, bounceDuration)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(bounceEase)
                    .SetUpdate(true)
                    .OnComplete(() => {
                        _currentTween = null;
                    });
            }
            else
            {
                // Un-highlight the button
                _selectable?.OnPointerExit(null);
            }
        }

        private void OnDestroy()
        {
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
                transform.localScale = _originalScale;
                _currentTween = null;
            }
        }
    }
}
