using TMPro;
using UnityEngine;

namespace com.zumstudios.zumpass
{
    public class ZUMProductCell : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleTMP;
        [SerializeField] private TextMeshProUGUI _codeTMP;
        [SerializeField] private TextMeshProUGUI _expireDateTMP;


        public static void Setup(ZUMPassProduct product, Transform transform)
        {
            var script = Instantiate(Resources.Load("ZUMProductCell") as GameObject, transform)
                .GetComponent<ZUMProductCell>();

            script.SetupInterface(product);
        }

        private void SetupInterface(ZUMPassProduct product)
        {
            _titleTMP.text = product.title;
            _codeTMP.text = product.code;
            _expireDateTMP.text = product.GetHumanReadableExpireDate();
        }
    }
}

