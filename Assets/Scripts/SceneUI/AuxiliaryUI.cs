using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneUI
{
    public class AuxiliaryUI : MonoBehaviour
    {
        [SerializeField] private Button clearButton;
        [SerializeField] private Button addButton;
        [SerializeField] private Button removeButton;
        [SerializeField] private TMP_InputField amountItemsInputField;
        [SerializeField] private TMP_Dropdown itemNamesDropDown;

        public string CurrentName => itemNamesDropDown.options[itemNamesDropDown.value].text;
        public int AmountItems { get; private set; }

        public Button ClearButton => clearButton;
        public Button AddButton => addButton;
        public Button RemoveButton => removeButton;
        public TMP_Dropdown ItemNamesDropDown => itemNamesDropDown;

        private void Awake()
        {
            amountItemsInputField.onValueChanged.AddListener(delegate(string value)
            {
                if (string.IsNullOrEmpty(value)) return;
                int.TryParse(value, out var amountItems);
                AmountItems = amountItems;
            });
        }

    }
}