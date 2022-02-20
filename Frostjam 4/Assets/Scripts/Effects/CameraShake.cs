using System.Collections;
using Core.ToolBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Effects
{
    public class CameraShake : Singleton<CameraShake>
    {
        [SerializeField] private float duration;
        [SerializeField] private float magnitude;

        public void Shake()
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }

        public void Shake(float durationNew, float magnitudeNew)
        {
            StartCoroutine(ShakeCoroutine(durationNew, magnitudeNew));
        }

        private IEnumerator ShakeCoroutine(float durationNew, float magnitudeNew)
        {
            var originalPos = transform.localPosition;
            var time = 0f;
            while (time < durationNew) {
                var newX = Random.Range(-1f, 1f) * magnitudeNew;
                var newY = Random.Range(-1f, 1f) * magnitudeNew;
                transform.localPosition = new Vector3(newX, newY, originalPos.z);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }
    }
}