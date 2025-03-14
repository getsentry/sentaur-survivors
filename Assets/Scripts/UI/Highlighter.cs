using System;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Highlighter : MonoBehaviour
    {
        [SerializeField] private float bounceStrength;
        [SerializeField] private float bounceDuration;
        [SerializeField] private Ease bounceEase;
        
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
                _selectable.OnPointerEnter(null);
                
                if (_currentTween != null && _currentTween.IsActive())
                {
                    _currentTween.Kill();
                    transform.localScale = _originalScale;
                }
                
                _currentTween = transform.DOScale(bounceStrength, bounceDuration)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(bounceEase)
                    .OnComplete(() => {
                        _currentTween = null;
                    });
            }
            else
            {
                _selectable.OnPointerExit(null);
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
