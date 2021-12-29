using System;
using System.Linq;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        private Collider2D[] _interactionResult = new Collider2D[10];


        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            int size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _mask);

            for (var i = 0; i < size; i += 1)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTags)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject);
                }
            }

        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {

        }
    }
}
