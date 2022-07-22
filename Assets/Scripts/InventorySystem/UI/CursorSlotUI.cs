using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem.UI
{
    public class CursorSlotUI : SlotUI
    {
        [SerializeField]
        private KeyCode splitKeyCode = KeyCode.LeftShift;
        private bool isDragged => !IsEmpty;
        public event EventHandler<CursorSlotEventArgs> CursorSlotUiChanged;

        public async void ChangeUiData(object sender, PointerEventData eventData)
        {
            var selectedSlotUI = (SlotUI) sender;
            transform.position = selectedSlotUI.transform.position;

            switch (selectedSlotUI.IsEmpty)
            {
                //Replace items
                case false when isDragged && ItemIcon != selectedSlotUI.ItemIcon:
                {
                    int.TryParse(AmountOfItemsUI, out var amount);
                    ItemIcon = selectedSlotUI.ItemIcon;
                    AmountOfItemsUI = selectedSlotUI.AmountOfItemsUI;
            
                    CursorSlotUiChanged?.Invoke(this,
                        new CursorSlotEventArgs(OperationType.Replace, selectedSlotUI.Index, amount));
                    return;
                }
                //Increase amount of items
                case false when isDragged:
                {
                    int.TryParse(AmountOfItemsUI, out var amount);
                    CursorSlotUiChanged?.Invoke(this,
                        new CursorSlotEventArgs(OperationType.Increase, selectedSlotUI.Index, amount));

                    return;
                }
                //Put item in empty slot 
                case true when isDragged:
                {
                    int.TryParse(AmountOfItemsUI, out var amount);
                    CursorSlotUiChanged?.Invoke(this,
                        new CursorSlotEventArgs(OperationType.Add, selectedSlotUI.Index, amount));

                    Clear();
                    return;
                }
                //Take item from selectedSlotUI
                case false when !isDragged:
                {
                    double.TryParse(selectedSlotUI.AmountOfItemsUI, out var amount);
            
                    //Take half items
                    if (Input.GetKey(splitKeyCode))
                    {
                        amount = Math.Ceiling(amount / 2.0f);
                    }

                    ItemIcon = selectedSlotUI.ItemIcon;
                    AmountOfItemsUI = amount.ToString(CultureInfo.InvariantCulture);

                    CursorSlotUiChanged?.Invoke(this,
                        new CursorSlotEventArgs(OperationType.Remove, selectedSlotUI.Index, (int)amount));
                    break;
                }
            }

            if (isDragged)
            {
                await FollowToMouse();
            }
        }

        private UniTask FollowToMouse()
        {
            return UniTask.WaitWhile(() =>
            {
                transform.position = Input.mousePosition;
                return isDragged;
            });
        }
    }
}