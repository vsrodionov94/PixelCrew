using UnityEngine;
using PixelCrew.Creatures;

namespace PixelCrew.Components
{
    public class ArmHeroComponent : MonoBehaviour
    {
        private Hero _hero;

        public void ArmHero(GameObject go)
        {
            Hero hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}
