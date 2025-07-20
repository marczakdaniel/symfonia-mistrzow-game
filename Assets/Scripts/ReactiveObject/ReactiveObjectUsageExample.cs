using R3;
using UnityEngine;

namespace ReactiveObjectTest
{
    /// <summary>
    /// Example MonoBehaviour showing how to use simplified ReactiveObject pattern
    /// </summary>
    public class ReactiveObjectUsageExample : MonoBehaviour
    {
        private ExampleReactiveClass _player;
        private CompositeDisposable _disposables = new();

        void Start()
        {
            // Create a reactive object
            _player = new ExampleReactiveClass();

            // Subscribe to any changes
            _player.OnChanged
                .Subscribe(_ => Debug.Log("Player object changed"))
                .AddTo(_disposables);

            // Test the reactive behavior
            TestReactiveBehavior();
        }

        void TestReactiveBehavior()
        {
            Debug.Log("=== Testing Reactive Behavior ===");

            // Change properties - these will trigger notifications
            _player.Name = "Hero";
            _player.Health = 100;
            _player.Speed = 5.5f;

            // Take damage - this will trigger health change
            _player.TakeDamage(20);

            // Try to set health above max - should be clamped
            _player.Health = 150;

            // Try to set health below min - should be clamped
            _player.Health = -10;
        }

        void OnDestroy()
        {
            // Clean up subscriptions
            _disposables?.Dispose();
            _player?.Dispose();
        }

        // Example of creating your own reactive class
        public class GamePlayer : ReactiveObject
        {
            private int _score;
            private bool _isAlive;
            private Vector3 _position;

            public int Score
            {
                get => _score;
                set
                {
                    if (_score != value)
                    {
                        _score = value;
                        NotifyChanged();
                    }
                }
            }

            public bool IsAlive
            {
                get => _isAlive;
                set
                {
                    if (_isAlive != value)
                    {
                        _isAlive = value;
                        NotifyChanged();
                    }
                }
            }

            public Vector3 Position
            {
                get => _position;
                set
                {
                    if (_position != value)
                    {
                        _position = value;
                        NotifyChanged();
                    }
                }
            }

            public void AddScore(int points)
            {
                Score += points;
            }

            public void Die()
            {
                IsAlive = false;
            }
        }
    }
} 