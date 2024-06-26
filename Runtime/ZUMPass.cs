using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace com.zumstudios.zumpass
{
    public class ZUMPass : MonoBehaviour
    {
        private string _application_key;
        private bool _initialized;
        private ZUMPassUser _user;

        public void Init(string application_key, Action onInitialized = null)
        {
            _application_key = application_key;

            var dirPath = DataHelper.GetPersistentDataPath(null);

            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }

            _initialized = true;

            onInitialized?.Invoke();
        }

        public bool IsInitialized() => _initialized;

        public bool IsLoggedIn()
        {
            if (_initialized)
            {
                var user = GetUser();
                return !(user == null || object.Equals(user, default(ZUMPassUser)));
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        public void Login(
            string email,
            string password,
            Action<ZUMPassUser> onSuccess,
            Action<string> onFail
        )
        {
            if (_initialized)
            {
                StartCoroutine(LoginCoroutine(email, password, onSuccess, onFail));
                return;
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        public void LogOut(Action onFinish)
        {
            var dirPath = DataHelper.GetPersistentDataPath(null);

            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
                Directory.CreateDirectory(dirPath);
            }

            _user = null;

            onFinish?.Invoke();
        }

        public ZUMPassUser GetUser()
        {
            if (_initialized)
            {
                if (_user == null || object.Equals(_user, default(ZUMPassUser)))
                {
                    _user = ZUMPassUser.Load();
                }

                return _user;
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        public void LoadProducts(Action<List<ZUMPassProduct>> onSuccess, Action<string> onFail)
        {
            if (_initialized)
            {
                var user = GetUser();

                if (user == null || object.Equals(user, default(ZUMPassUser)))
                {
                    throw new Exception("ZUMPassUser não está logado!");
                }

                StartCoroutine(LoadProductsCoroutine(user, onSuccess, onFail));
                return;
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        public List<ZUMPassProduct> GetAvailableProducts()
        {
            if (_initialized)
            {
                var response = ZUMPassProductResponse.Load();
                return response.products;
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        public ZUMPassProduct GetProductWithID(string product_id)
        {
            if (_initialized)
            {
                try
                {
                    var products = GetAvailableProducts();
                    return products.FirstOrDefault(
                        product => string.Equals(product.product_id, product_id)
                    );
                }
                catch (Exception)
                {
                    return null;
                }
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        public bool IsProductAvailable(string product_id)
        {
            if (_initialized)
            {
                var obj = GetProductWithID(product_id);

                if (obj == null || obj.Equals(default(ZUMPassProduct)))
                {
                    return false;
                }

                return obj.HasExpired() == false;
            }

            throw new Exception("ZUMPassManager não foi inicializado!");
        }

        private IEnumerator LoginCoroutine(
            string email,
            string password,
            Action<ZUMPassUser> onSuccess,
            Action<string> onFail
        )
        {
            UnityWebRequest request = UnityWebRequest.Post(ZUMPassConstants.LOGIN_ENDPOINT, "POST");
            var login = new ZUMPassLogin(email, password);
            var bodyRaw = Encoding.UTF8.GetBytes(login.ToJSON());

            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<ZUMPassLoginResponse>(
                    request.downloadHandler.text
                );

                if (response.success)
                {
                    var passUser = new ZUMPassUser(email, password, response.token);
                    passUser.Save();
                    onSuccess?.Invoke(passUser);
                }
                else
                {
                    onFail?.Invoke(response.msg);
                }
            }
            else if (request.result == UnityWebRequest.Result.InProgress) { }
            else
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<ZUMPassLoginResponse>(
                        request.downloadHandler.text
                    );
                    onFail?.Invoke(response.msg);
                }
                catch (Exception)
                {
                    onFail?.Invoke(request.downloadHandler.error);
                }
            }
        }

        private IEnumerator LoadProductsCoroutine(
            ZUMPassUser user,
            Action<List<ZUMPassProduct>> onSuccess,
            Action<string> onFail
        )
        {
            UnityWebRequest request = UnityWebRequest.Post(
                ZUMPassConstants.PRODUCTS_ENDPOINT,
                "POST"
            );

            var body = new Dictionary<string, string>();
            body.Add("application_key", _application_key);
            body.Add("token", user.token);

            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<ZUMPassProductResponse>(
                    request.downloadHandler.text
                );

                if (response.success)
                {
                    response.Save();
                    onSuccess?.Invoke(response.products);
                }
                else
                {
                    onFail?.Invoke(response.msg);
                }
            }
            else if (request.result == UnityWebRequest.Result.InProgress) { }
            else
            {
                try
                {
                    var response = JsonConvert.DeserializeObject<ZUMPassProductResponse>(
                        request.downloadHandler.text
                    );
                    onFail?.Invoke(response.msg);
                }
                catch (Exception)
                {
                    onFail?.Invoke(request.downloadHandler.error);
                }
            }
        }

        private static readonly object padlock = new object();
        private static ZUMPass _instance;
        public static ZUMPass Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        var go = new GameObject();
                        go.name = "@ZUMPass";
                        _instance = go.AddComponent<ZUMPass>();
                        DontDestroyOnLoad(go);
                    }

                    return _instance;
                }
            }
        }
    }
}
