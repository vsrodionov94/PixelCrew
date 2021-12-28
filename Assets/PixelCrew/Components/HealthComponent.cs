using UnityEngine;
using UnityEngine.Events;
using System;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private HealthChange _onChange;

        public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;

            _health += healthDelta;

            _onChange?.Invoke(_health);
            if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if (healthDelta > 0)
            {
                _onHeal?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void SetHealth(int currentHealth)
        {
            _health = currentHealth;
        }

        [Serializable]
        public class HealthChange : UnityEvent<int>
        {
        }
    }
}
