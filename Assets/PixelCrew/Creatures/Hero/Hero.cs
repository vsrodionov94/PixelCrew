using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private GameSession _session;
        private HealthComponent _healthComponent;


        private bool _allowDoubleJump;
        private bool _isOnWall;
        private float _defaultGravityScale;
        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWallKey = Animator.StringToHash("is-on-wall");

        protected override void Awake()
        {
            base.Awake();
            _healthComponent = GetComponent<HealthComponent>();
            _defaultGravityScale = Rigidbody.gravityScale;
        }

        public void Throw()
        {
            if (_throwCooldown.IsReady)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }

        public void OnDoThrow()
        {
            Particles.Spawn("Throw");
        }

        public void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session != null)
            {
                _healthComponent?.SetHealth(_session.Data.Hp);
            }
            UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }


        protected override float CalculateYVelocity()
        {
            bool isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                Particles.Spawn("Jump");
                _allowDoubleJump = false;
                return JumpSpeed;
            }
            return base.CalculateJumpVelocity(yVelocity);
        }

        protected override void Update()
        {
            base.Update();

            var moveToSomeDirection = Direction.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSomeDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            
            Animator.SetBool(IsOnWallKey, _isOnWall);
        }

        public void AddCoins(int coins)
        {
            _session.Data.Coins += coins;
            Debug.Log($"{coins} coins added. total coins: {_session.Data.Coins}");
        }

        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDispose;

            ParticleSystem.Burst burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;
            Particles.Spawn("SwordSplash");
            base.Attack();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(GroundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    Particles.Spawn("SlamDown");
                }

                if (contact.relativeVelocity.y >= DamageVelocity)
                {
                    _healthComponent.ModifyHealth(-1);
                }
            }
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
        }
    }
}

