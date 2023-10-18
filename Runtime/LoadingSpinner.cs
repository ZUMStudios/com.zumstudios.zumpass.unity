using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.zumstudios.zumpass
{
    public class LoadingSpinner : MonoBehaviour
    {
        private RectTransform _rectComponent = null;
        private Image _spinnerImg = null;
        private bool _isUp;
        private bool _isRotating = false;

        public float RotateSpeed = 200f;
        public float OpenSpeed = .005f;
        public float CloseSpeed = .01f;

        private void Start()
        {
            _rectComponent = GetComponent<RectTransform>();
            _spinnerImg = _rectComponent.GetComponent<Image>();
            _isUp = true;

            StartCoroutine(RandomStartSpinning());
        }

        private void Update()
        {
            if (_isRotating)
            {
                _rectComponent.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
                Rotate();
            }
        }

        private IEnumerator RandomStartSpinning()
        {
            yield return new WaitForSeconds(Random.Range(0, 0.3f));

            _isRotating = true;
        }

        private void Rotate()
        {
            float currentSize = _spinnerImg.fillAmount;

            if (currentSize < .70f && _isUp)
            {
                _spinnerImg.fillAmount += OpenSpeed;
            }
            else if (currentSize >= .30f && _isUp)
            {
                _isUp = false;
            }
            else if (currentSize >= .02f && !_isUp)
            {
                _spinnerImg.fillAmount -= CloseSpeed;
            }
            else if (currentSize < .02f && !_isUp)
            {
                _isUp = true;
            }
        }
    }
}
