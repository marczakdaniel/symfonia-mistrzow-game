using R3;
using UnityEngine;

namespace ReactiveObjectTest
{
    /// <summary>
    /// Example class demonstrating how to use simplified ReactiveObject as a base class
    /// </summary>
    public class ExampleReactiveClass : ReactiveObject
    {
        private string _name;
        private int _health;
        private float _speed;

        /// <summary>
        /// Property that manually notifies when changed
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyChanged();
                }
            }
        }

        /// <summary>
        /// Property that manually notifies when changed
        /// </summary>
        public int Health
        {
            get => _health;
            set
            {
                if (_health != value)
                {
                    _health = value;
                    OnHealthChanged(value);
                    NotifyChanged();
                }
            }
        }

        /// <summary>
        /// Property that manually notifies when changed
        /// </summary>
        public float Speed
        {
            get => _speed;
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    NotifyChanged();
                }
            }
        }

        public ExampleReactiveClass()
        {
            // Subscribe to any changes
            //OnChanged
            //    .Subscribe(_ => Debug.Log("Some property changed"))
            //    .AddTo(this);
        }

        private void OnHealthChanged(int newHealth)
        {
            Debug.Log($"Health changed to: {newHealth}");
            
            // Example: Clamp health to valid range
            if (newHealth < 0)
            {
                _health = 0;
            }
            else if (newHealth > 100)
            {
                _health = 100;
            }
        }

        /// <summary>
        /// Example method that changes multiple properties
        /// </summary>
        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Example method that changes a property manually
        /// </summary>
        public void SetName(string newName)
        {
            if (_name != newName)
            {
                _name = newName;
                NotifyChanged();
            }
        }
    }
} 