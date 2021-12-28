using UnityEngine;

namespace PixelCrew.Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _healthDelta;

        public void Apply(GameObject target)
        {
            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ModifyHealth(_healthDelta);
            }

        }
    }
}
