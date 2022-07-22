namespace InventorySystem.UI
{
    public class CursorSlotEventArgs : System.EventArgs
    {
        public CursorSlotEventArgs(OperationType operationType, int index, int amount)
        {
            OperationType = operationType;
            Index = index;
            Amount = amount;
        }

        public OperationType OperationType { get; }
        public int Index { get; }
        public int Amount { get; }
    }
}