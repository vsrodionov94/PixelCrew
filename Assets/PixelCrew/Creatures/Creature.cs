using UnityEngine;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;

namespace PixelCrew.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float JumpSpeed;
        [SerializeField] protected float DamageVelocity;
        [SerializeField] private int _damage;

        [Header("Checkers")]
        [SerializeField] protected LayerMask GroundLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent Particles;

        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected bool IsGrounded;
        protected Vector2 Direction;
        protected bool IsJumping;


        protected static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        protected static readonly int IsRunningKey = Animator.StringToHash("is-running");
        protected static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        protected static readonly int HitKey = Animator.StringToHash("hit");
        protected static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        private void FixedUpdate()
        {
            float xVelocity = Direction.x * _speed;
            float yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetBool(IsRunningKey, Direction.x != 0);

            UpdateSpriteDirection(Direction);
        }


        protected virtual float CalculateYVelocity()
        {
            float yVelocity = Rigidbody.velocity.y;
            bool isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                IsJumping = false;
            }
            if (isJumpPressing)
            {
                IsJumping = true;
                
                bool isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && IsJumping)
            {
                yVelocity *= 0.5f;
            }
            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity = JumpSpeed;
                Particles.Spawn("Jump");
            }
            return yVelocity;
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            float multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-multiplier, 1, 1);
            }
        }

        public virtual void TakeDamage()
        {
            IsJumping = false;
            Animator.SetTrigger(HitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, DamageVelocity);
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }

        public void OnDoAttack()
        {
            _attackRange.Check();
        }
    }
}
