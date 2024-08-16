using Assets.Scripts;
using Assets.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Items
{
    public class Inventory
    {

        public readonly EntityPlayer Holder;

        public readonly ItemStack[] Items;

        public Inventory(EntityPlayer holder)
        {
            Holder = holder;
            Items = new ItemStack[Holder.InventorySize];
        }

        public ItemStack GetItem(int index)
        {
            return Items[index];
        }

        public int AmountOf(Material material) 
        {
            int amount = 0;
            foreach(ItemStack item in Items)
            {
                if (item == null) continue;
                if(item.Material == material) amount += item.Amount;
            }
            return amount;
        }

        public int FreeSpaceFor(Material material)
        {
            int amount = 0;
            foreach (ItemStack item in Items)
            {
                if (item == null)
                {
                    amount += ItemStack.MAX_AMOUNT;
                    continue;
                }
                if (item.Material == material) amount += ItemStack.MAX_AMOUNT - item.Amount;
            }
            return amount;
        }


        public void SetItem(ItemStack item, int index)
        {
            ItemStack old = Items[index];
            Items[index] = item;
            if(index == Holder.CurrentSlot)
            {
                if(old != null && old.Material == Material.GUN && item != null && item.Material != Material.GUN)
                {
                    Holder.GunHud = false;
                } else if(old == null && item != null && item.Material == Material.GUN)
                {
                    Holder.GunHud = true;
                }
            }
            if(Holder.gameManager.MainPlayer == Holder)
            {
                Holder.playerInterface.UpdateSlot(index);
            }
        }

        public bool SubstractItem(int index, int amount)
        {
            ItemStack item = GetItem(index);
            if (item == null) return false;
            if (item.Amount < amount) return false;
            if (item.Amount == amount)
            {
                SetItem(null, index);
            }
            else
            {
                ItemStack newItem = new ItemStack(item.Material, item.Amount - amount);
                SetItem(newItem, index);
            }
            return true;
        }

        public bool PlusItem(int index, int amount)
        {
            ItemStack item = GetItem(index);
            if(item == null) return false;
            if(item.Amount + amount > ItemStack.MAX_AMOUNT) return false;
            SetItem(new ItemStack(item.Material, item.Amount + amount), index);
            return true;
        }

        public bool AddItem(Material material, int amount)
        {
            if (FreeSpaceFor(material) < amount) return false;
            for (int i = 0; i < Holder.InventorySize; i++)
            {
                if (amount == 0) break;
                ItemStack item = Items[i];
                if(item == null)
                {
                    if(amount <= ItemStack.MAX_AMOUNT)
                    {
                        SetItem(new ItemStack(material, amount), i);
                        amount = 0;
                    } else
                    {
                        SetItem(new ItemStack(material, ItemStack.MAX_AMOUNT), i);
                        amount -= ItemStack.MAX_AMOUNT;
                    }
                } else
                {
                    if(item.Material == material && item.Amount < ItemStack.MAX_AMOUNT)
                    {
                        int free = ItemStack.MAX_AMOUNT - item.Amount;
                        if(free >= amount)
                        {
                            SetItem(new ItemStack(material, item.Amount + amount), i);
                            amount = 0;
                        } else
                        {
                            SetItem(new ItemStack(material, ItemStack.MAX_AMOUNT), i);
                            amount -= free;
                        }
                    }
                }
            }
            return true;
        }

        public bool TakeItems(Material material, int amount)
        {
            if (amount < 0) return false;
            if (AmountOf(material) < amount) return false;
            for(int i = Holder.InventorySize - 1; i >= 0; i--)
            {
                if (amount == 0) break;
                ItemStack item = Items[i];
                if (item == null) continue;
                if (item.Material != material) continue;
                if(item.Amount <= amount)
                {
                    amount -= item.Amount;
                    SetItem(null, i);
                } else
                {
                    SetItem(new ItemStack(material, item.Amount - amount), i);
                    amount = 0;
                }
            }
            return true;
        }

    }
}
