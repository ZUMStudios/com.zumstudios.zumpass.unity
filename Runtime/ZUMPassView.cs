using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.zumstudios.zumpass
{
    public class ZUMPassView : MonoBehaviour, IZUMPassLogin, IZUMPassProductLoad
    {
        #region Variables

        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _loadingSpinner;

        [SerializeField] private GameObject _loginGO;
        [SerializeField] private TMP_InputField _usernameField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private TextMeshProUGUI _messageTMP;

        [SerializeField] private GameObject _loggedGO;
        [SerializeField] private Transform _scrollContentTransform;
        [SerializeField] private TextMeshProUGUI _headerTMP;
        [SerializeField] private TextMeshProUGUI _noOffersTMP;

        [SerializeField] private Animator _animator;

        private Action _onSuccess;
        private Action<string> _onFailed;
        private Action _onLogout;

        #endregion

        #region Static methods

        public static void Show(Action onSuccess, Action<string> onFailed, Action onLogout, int layerOrder = 32700)
        {
            var script = Instantiate(Resources.Load("ZUMPassView") as GameObject).GetComponent<ZUMPassView>();

            script._onSuccess = onSuccess;
            script._onFailed = onFailed;
            script._onLogout = onLogout;
            script.SetupInterface(layerOrder);
            script._animator.SetBool("Show", true);
        }

        #endregion

        #region Private methods

        private void SetupInterface(int layerOrder)
        {
            _canvas.sortingOrder = layerOrder;
            _loadingSpinner.SetActive(false);
            _messageTMP.text = "";

            if (ZUMPass.Instance.IsLoggedIn())
            {
                _loginGO.SetActive(false);
                _loggedGO.SetActive(true);
                
                SetupProducts(ZUMPass.Instance.GetAvailableProducts());
            }
            else
            {
                _loginGO.SetActive(true);
                _loggedGO.SetActive(false);
            }
        }

        private void SetupProducts(List<ZUMPassProduct> products)
        {
            _headerTMP.text = $"Usuário logado como:\n<b>{ZUMPass.Instance.GetUser().email}</b>";

            foreach(var product in products)
            {
                ZUMProductCell.Setup(product, _scrollContentTransform);
            }

            _noOffersTMP.gameObject.SetActive(products.Count == 0);
        }

        private void DestroyAllCells(Action onComplete)
        {
            foreach (Transform child in _scrollContentTransform)
            {
                Destroy(child.gameObject);
            }

            onComplete?.Invoke();
        }

        #endregion

        #region Animator Event

        // Animator Event method
        public void OnHideAnimationEnd()
        {
            Destroy(gameObject);
        }

        #endregion

        #region Button pressed methods

        public void OnLoginButtonPressed()
        {
            _loadingSpinner.gameObject.SetActive(true);

            ZUMPass.Instance.Login(
                _usernameField.text, _passwordField.text, OnLoginSuccess, OnLoginFail);
        }

        public void OnLogOutButtonPressed()
        {
            ZUMPass.Instance.LogOut(() =>
            {
                _loginGO.SetActive(true);
                _loggedGO.SetActive(false);
                _onLogout?.Invoke();
            });
        }

        public void OnHideButtonPressed()
        {
            _animator.SetBool("Show", false);
        }

        #endregion

        #region IZUMPassLogin
        public void OnLoginSuccess(ZUMPassUser user)
        {
            ZUMPass.Instance.LoadProducts(OnProductLoadSuccess, OnProductLoadFail);
        }

        public void OnLoginFail(string message)
        {
            _loadingSpinner.SetActive(false);
            _messageTMP.text = message;
        }

        public void OnProductLoadSuccess(List<ZUMPassProduct> products)
        {
            DestroyAllCells(() =>
            {
                SetupProducts(products);
                _loadingSpinner.SetActive(false);
                _loginGO.SetActive(false);
                _loggedGO.SetActive(true);

                _onSuccess?.Invoke();
            });
        }

        public void OnProductLoadFail(string message)
        {
            ZUMPass.Instance.LogOut(() =>
            {
                _animator.SetBool("Show", false);
                _onFailed?.Invoke(message);
            });
        }
        #endregion
    }
}

