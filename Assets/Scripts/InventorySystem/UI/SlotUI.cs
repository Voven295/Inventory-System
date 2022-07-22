using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected TMP_Text amountOfItemsUI;
    
        public event EventHandler<PointerEventData> SlotClicked;
        public bool IsEmpty => !itemIcon.gameObject.activeSelf;
        private int index;
        public int Index => index;

        private void Awake()
        {
            index = transform.GetSiblingIndex();
        }

        public string AmountOfItemsUI
        {
            get => amountOfItemsUI.text;
            set
            {
                if (value == "0")
                {
                    Clear();
                    return;
                }
                amountOfItemsUI.text = value;
            }
        }
    
        public Sprite ItemIcon
        {
            get => itemIcon.sprite;
            set
            {
                itemIcon.sprite = value;
                bool isValueNull = value == null;
            
                itemIcon.gameObject.SetActive(!isValueNull);
            }
        }

        protected void Clear()
        {
            ItemIcon = null;
            AmountOfItemsUI = string.Empty;
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            SlotClicked?.Invoke(this, eventData);
        }
    }
}