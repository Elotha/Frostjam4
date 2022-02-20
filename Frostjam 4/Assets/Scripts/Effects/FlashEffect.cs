using System.Collections;
using UnityEngine;

namespace Effects
{
    public class FlashEffect : MonoBehaviour
    {
        [SerializeField] private float flashTime = 0.1f;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material flashMaterial;

        private SpriteRenderer _sprRenderer;
        private void Awake()
        {
            _sprRenderer = GetComponent<SpriteRenderer>();
        }

        public void StartFlashing()
        {
            StartCoroutine(ColorChange());
        }

        private IEnumerator ColorChange()
        {
            var color = _sprRenderer.color;
            _sprRenderer.color = Color.white;
            _sprRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashTime);
            _sprRenderer.material = defaultMaterial;
            _sprRenderer.color = color;
        }
    }
}