using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Items
{
    public class ItemStack
    {

        public const int MAX_AMOUNT = 8;

        private Material _material;
        private int _amount;

        public Material Material
        {

            get
            {
                return _material;
            }
            set
            {
                _material = value;
            }
        }
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public ItemStack(Material material, int amount)
        {
            _material = material;
            _amount = amount;
        }


    }
}