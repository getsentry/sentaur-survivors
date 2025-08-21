using UnityEngine;

namespace Characters
{
    public class RandomEnemy : Enemy
    {
        private Vector2 _targetPosition;
        private float _stoppingDistance = 0.5f; // How close to the target before picking a new one
        private Camera _mainCamera;
        private float _mapBorderOffset = 2f; // Keep targets away from screen edges

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
            PickNewRandomTarget();
        }

        protected override Vector2 DetermineDirection(GameObject player)
        {
            var distanceToTarget = Vector2.Distance(transform.position, _targetPosition);
            
            if (distanceToTarget <= _stoppingDistance)
            {
                PickNewRandomTarget();
            }

            var direction = (_targetPosition - (Vector2)transform.position).normalized;
            return direction;
        }

        private void PickNewRandomTarget()
        {
            var bottomLeft = _mainCamera.ScreenToWorldPoint(Vector3.zero);
            var topRight = _mainCamera.ScreenToWorldPoint(new Vector3(_mainCamera.pixelWidth, _mainCamera.pixelHeight, 0));

            var minX = bottomLeft.x + _mapBorderOffset;
            var maxX = topRight.x - _mapBorderOffset;
            var minY = bottomLeft.y + _mapBorderOffset;
            var maxY = topRight.y - _mapBorderOffset;

            _targetPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );
        }
    }
}