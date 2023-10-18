using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.zumstudios.zumpass
{
    public class ZUMPassView : MonoBehaviour, IZUMPassLogin, IZUMPassProductLoad
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _backgroundCG;
        [SerializeField] private GameObject _contentGO;
        [SerializeField] private LoadingSpinner _loadingSpinner;

        [SerializeField] private GameObject _loginGO;
        [SerializeField] private TMP_InputField _usernameField;
        [SerializeField] private TMP_InputField _passwordField;


        [SerializeField] private GameObject _loggedGO;
        [SerializeField] private Transform _scrollContentTransform;
        [SerializeField] private TextMeshProUGUI _headerTMP;
        [SerializeField] private TextMeshProUGUI _noOffersTMP;

        private Action<string> _onError;

        public static void Show(Action<string> onError, int layerOrder = 32767)
        {
            var script = Instantiate(Resources.Load("ZUMPassView") as GameObject).GetComponent<ZUMPassView>();

            script._onError = onError;
            script.SetupInterface(layerOrder);
            script.AnimateShow();
        }

        private void SetupInterface(int layerOrder)
        {
            _canvas.sortingOrder = layerOrder;

            _backgroundCG.alpha = 0f;
            _backgroundCG.interactable = false;

            _contentGO.transform.localScale = Vector3.zero;

            if (ZUMPass.Instance.IsLoggedIn())
            {
                _loginGO.SetActive(false);
                _loggedGO.SetActive(true);
                _loadingSpinner.gameObject.SetActive(true);

                ZUMPass.Instance.LoadProducts(OnProductLoadSuccess, OnProductLoadFail);
            }
            else
            {
                _loginGO.SetActive(true);
                _loggedGO.SetActive(false);
                _loadingSpinner.gameObject.SetActive(false);
            }
        }

        private void SetupProductsScrollView(List<ZUMPassProduct> products)
        {
            foreach(var product in products)
            {
                ZUMProductCell.Setup(product, _scrollContentTransform);
            }
        }

        private void DestroyAllCells(Action onComplete)
        {
            foreach (Transform child in _scrollContentTransform)
            {
                Destroy(child.gameObject);
            }

            onComplete?.Invoke();
        }

        private void AnimateShow()
        {
            LeanTween.alphaCanvas(_backgroundCG, 1f, 0.2f).setOnComplete(() =>
            {
                _backgroundCG.interactable = true;
            });

            LeanTween.scale(_contentGO, Vector3.one, 0.2f).setDelay(0.1f);
        }

        private void AnimateHide()
        {
            _backgroundCG.interactable = false;

            LeanTween.scale(_contentGO, Vector3.zero, 0.2f);
            LeanTween.alphaCanvas(_backgroundCG, 0f, 0.2f).setDelay(0.1f).setOnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public void OnLoginButtonPressed()
        {
            _loadingSpinner.gameObject.SetActive(true);

            ZUMPass.Instance.Login(_usernameField.text, _passwordField.text, OnLoginSuccess, OnLoginFail);
        }

        public void OnLogOutButtonPressed()
        {
            ZUMPass.Instance.LogOut(() =>
            {
                _loginGO.SetActive(true);
                _loggedGO.SetActive(false);
            });
        }

        public void OnUpdateButtonPressed()
        {
            ZUMPass.Instance.LoadProducts(OnProductLoadSuccess, OnProductLoadFail);
        }

        #region IZUMPassLogin
        public void OnLoginSuccess(ZUMPassUser user)
        {
            ZUMPass.Instance.LoadProducts(OnProductLoadSuccess, OnProductLoadFail);
        }

        public void OnLoginFail(string message)
        {
            _loadingSpinner.gameObject.SetActive(false);
            _onError?.Invoke(message);
        }

        public void OnProductLoadSuccess(List<ZUMPassProduct> products)
        {
            DestroyAllCells(() =>
            {
                SetupProductsScrollView(products);
                _loadingSpinner.gameObject.SetActive(false);
                _loginGO.SetActive(false);
                _loggedGO.SetActive(true);
            });
        }

        public void OnProductLoadFail(string message)
        {
            _loadingSpinner.gameObject.SetActive(false);
            _onError?.Invoke(message);
        }
        #endregion
    }
}

