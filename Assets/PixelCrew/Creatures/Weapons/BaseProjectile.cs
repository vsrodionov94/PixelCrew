using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float Speed;
        [SerializeField] protected bool InvertX;
        
        protected Rigidbody2D Rigidbody;
        protected float Direction;
        
        protected virtual void Start()
        {
            var mod = InvertX ? -1 : 1;
            Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}