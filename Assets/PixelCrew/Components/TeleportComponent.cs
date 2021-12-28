using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        [SerializeField] private float _alphaTime = 1;
        [SerializeField] private float _moveTime = 1;

        public void Teleport(GameObject target)
        {
            // target.transform.position = _destTransform.position;
            StartCoroutine(AnimateTeleport(target));
        }

        private IEnumerator AnimateTeleport(GameObject target)
        {
            SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();
            PlayerInput input = target.GetComponent<PlayerInput>();

            SetLockInput(true, input);

            yield return AlphaAnimation(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);


            target.SetActive(true);
            yield return AlphaAnimation(sprite, 1);
            SetLockInput(false, input);
        }

        private void SetLockInput(bool isLocked, PlayerInput input)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            float alphaTime = 0f;
            float spriteAlpha = sprite.color.a;
            while (alphaTime < _alphaTime)
            {
                alphaTime += Time.deltaTime;
                float progress = alphaTime / _alphaTime;
                float tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress);
                Color color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                yield return null;
            }
        }

        private IEnumerator MoveAnimation(GameObject target)
        {
            float moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                float progress = moveTime / _alphaTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

                yield return null;
            }
        }
    }
}
