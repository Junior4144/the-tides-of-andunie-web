using System;

[Serializable]
public class InventorySlot
{
    public IInventoryItem Item { get; private set; }
    public int Quantity { get; private set; }

    public InventorySlot(IInventoryItem item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }

    public bool AddQuantity(int amount)
    {
        if (!Item.IsStackable)
            return false;

        int newQuantity = Quantity + amount;
        if (newQuantity > Item.MaxStackSize)
            return false;

        Quantity = newQuantity;
        return true;
    }

    public bool RemoveQuantity(int amount)
    {
        if (amount > Quantity)
            return false;

        Quantity -= amount;
        return true;
    }

    public bool CanAddQuantity(int amount)
    {
        if (!Item.IsStackable)
            return false;

        return (Quantity + amount) <= Item.MaxStackSize;
    }

    public bool IsEmpty()
    {
        return Quantity <= 0;
    }
}