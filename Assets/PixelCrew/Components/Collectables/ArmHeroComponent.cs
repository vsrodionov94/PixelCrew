using PixelCrew.Creatures;
using PixelCrew.Creatures.Hero;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class ArmHeroComponent : MonoBehaviour
    {
        private Hero _hero;

        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.ArmHero();
            }
        }
    }
}
