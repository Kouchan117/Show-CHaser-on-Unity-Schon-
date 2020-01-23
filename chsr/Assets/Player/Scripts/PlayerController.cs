using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private AnimationCurve moveCurve = default;

        private Animator animator = default;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void Move(Vector3 position)
        {
            StopCoroutine(MoveCoroutine(position, moveTime));
            StopCoroutine(RotateCoroutine(position - transform.position, moveTime));


            StartCoroutine(MoveCoroutine(position, moveTime));
            StartCoroutine(RotateCoroutine(position - transform.position, moveTime / 2f));
        }

        private IEnumerator MoveCoroutine(Vector3 position, float time)
        {
            //yield return RotateCoroutine(position - transform.position, time);

            float t = 0;
            var a = transform.position;
            var b = position;

            if (a == b) yield break;

            animator.SetBool("isWalk", true);

            while (t <= 1)
            {
                transform.position = Vector3.Lerp(a, b, moveCurve.Evaluate(t));
                yield return null;
                t += Time.deltaTime / time;
            }

            transform.position = b;
            animator.SetBool("isWalk", false);
            yield break;
        }

        private IEnumerator RotateCoroutine(Vector3 target, float time)
        {
            float t = 0;
            var a = transform.rotation;
            var b = Quaternion.LookRotation(target, Vector3.up);

            if (a == b) yield break;
            
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