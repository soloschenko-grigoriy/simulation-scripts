using UnityEngine;
using UnityEngine.UI;

namespace GS.UI
{
    public class Item : MonoBehaviour
    {
        private Text _textElm;

        public string Value
        {
            get => _textElm.text;
            set => _textElm.text = value;
        }

        private void Start()
        {
            _textElm = GetComponent<Text>();
        }
    }
}
