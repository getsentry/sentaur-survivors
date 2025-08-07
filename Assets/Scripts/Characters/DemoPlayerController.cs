using System;
using UnityEngine;

namespace Characters
{
    public class DemoPlayerController : MonoBehaviour
    {
        private DemoConfiguration _demoConfig;
        private Rigidbody2D _rigidBody;
        private Transform _enemiesTransform;
        private Transform _pickupsTransform;
        private Transform _xpDropsTransform;
        private Dart _dart;
        
        [SerializeField]
        private float _moveSpeed = 5f;

        private void Awake()
        {
            _demoConfig = Resources.Load("DemoConfig") as DemoConfiguration;
        }

        private void Start()
        {
            // Get the RigidBody2D from the GameObject
            _rigidBody = GetComponent<Rigidbody2D>();
            
            // Find the Level GameObject and get references to child transforms
            GameObject levelObject = GameObject.Find("Level");
            if (levelObject != null)
            {
                _enemiesTransform = levelObject.transform.Find("Enemies");
                _pickupsTransform = levelObject.transform.Find("Pickups");
                _xpDropsTransform = levelObject.transform.Find("XpDrops");
            }
            else
            {
                Debug.LogWarning("Level GameObject not found!");
            }
            
            // Get the Dart component from child transforms
            _dart = GetComponentInChildren<Dart>();
            if (_dart == null)
            {
                Debug.LogWarning("Dart component not found in children!");
            }
        }

        private void Update()
        {
            if (_demoConfig != null && _demoConfig.AutoPlay)
            {
                HandleMovement();
                HandleShooting();    
            }
        }

        private void HandleMovement()
        {
            Transform targetTransform = GetClosestXpDrop();
            
            // If no XpDrop found, look for closest Pickup
            if (targetTransform == null)
            {
                targetTransform = GetClosestPickup();
            }
            
            if (targetTransform != null)
            {
                MoveTowardsTarget(targetTransform);
            }
        }

        private void HandleShooting()
        {
            if (_dart != null)
            {
                Transform closestEnemy = GetClosestEnemy();
                if (closestEnemy != null)
                {
                    Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
                    _dart.SetShootingDirection(directionToEnemy);
                }
            }
        }

        private Transform GetClosestXpDrop()
        {
            if (_xpDropsTransform == null || _xpDropsTransform.childCount == 0)
                return null;

            Transform closest = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < _xpDropsTransform.childCount; i++)
            {
                Transform child = _xpDropsTransform.GetChild(i);
                float distance = Vector3.Distance(transform.position, child.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = child;
                }
            }

            return closest;
        }

        private Transform GetClosestPickup()
        {
            if (_pickupsTransform == null || _pickupsTransform.childCount == 0)
                return null;

            Transform closest = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < _pickupsTransform.childCount; i++)
            {
                Transform child = _pickupsTransform.GetChild(i);
                float distance = Vector3.Distance(transform.position, child.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = child;
                }
            }

            return closest;
        }

        private Transform GetClosestEnemy()
        {
            if (_enemiesTransform == null || _enemiesTransform.childCount == 0)
                return null;

            Transform closest = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < _enemiesTransform.childCount; i++)
            {
                Transform child = _enemiesTransform.GetChild(i);
                float distance = Vector3.Distance(transform.position, child.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = child;
                }
            }

            return closest;
        }

        private void MoveTowardsTarget(Transform target)
        {
            if (_rigidBody != null && target != null)
            {
                var direction = (target.position - transform.position).normalized;
                _rigidBody.linearVelocity = direction * _moveSpeed;
            }
        }
    }
}