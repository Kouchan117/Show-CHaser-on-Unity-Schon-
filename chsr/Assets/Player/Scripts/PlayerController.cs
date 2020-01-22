using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private AnimationCurve moveCurve = default;

        public void Move(Vector3 position)
        {
            StopCoroutine(MoveCoroutine(position, moveTime / 2f));

            StartCoroutine(MoveCoroutine(position, moveTime / 2f));
        }

        private IEnumerator MoveCoroutine(Vector3 position, float time)
        {
            yield return RotateCoroutine(position - transform.position, time);

            float t = 0;
            var a = transform.position;
            var b = position;

            while(t <= 1)
            {
                transform.position = Vector3.Lerp(a, b, moveCurve.Evaluate(t));
                yield return null;
                t += Time.deltaTime / time;
            }

            transform.position = b;
            yield break;
        }

        private IEnumerator RotateCoroutine(Vector3 target, float time)
        {
            float t = 0;
            var a = transform.rotation;
            var b = Quaternion.LookRotation(target, Vector3.up);

            while (t <= 1)
            {
                transform.rotation = Quaternion.Lerp(a, b, moveCurve.Evaluate(t));
                yield return null;
                t += Time.deltaTime / time;
            }

            transform.rotation = b;
            yield break;
        }
    }
}