using Assets.Scripts.Blocks;
using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerInterface : MonoBehaviour
    {

        private GameManager gameManager;
        private ResourceStorage resourceStorage;

        private GameObject InvObject;
        private RectTransform InvRect;
        private GameObject[] Slots;
        private RectTransform[] SlotRects;
        private Image[] SlotBgs;
        private GameObject[] SlotImages;
        private Image[] SlotImageComps;
        private GameObject[] SlotAmounts;
        private Text[] SlotAmountComps;

        private GameObject shop;
        private GameObject goldCount;
        private Text goldCountText;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();

            shop = transform.Find("Shop").gameObject;
            goldCount = shop.transform.Find("GoldCount").gameObject;
            goldCountText = goldCount.GetComponent<Text>();
        }

        public bool OpenedShop
        {
            get
            {
                return shop.activeSelf;
            }
            set
            {
                shop.SetActive(value);
            }
        }

        public void SetGoldCount(int count)
        {
            goldCountText.text = count.ToString();
        }

        public void Initialize()
        {
            InvObject = GameManager.Instantiate(resourceStorage.InventoryPrefab, transform);
            InvRect = InvObject.GetComponent<RectTransform>();
            InvRect.sizeDelta = new Vector2(55f * gameManager.MainPlayer.InventorySize + 5f, 60f);

            Slots = new GameObject[gameManager.MainPlayer.InventorySize];
            SlotRects = new RectTransform[gameManager.MainPlayer.InventorySize];
            SlotBgs = new Image[gameManager.MainPlayer.InventorySize];
            SlotImages = new GameObject[gameManager.MainPlayer.InventorySize];
            SlotImageComps = new Image[gameManager.MainPlayer.InventorySize];
            SlotAmounts = new GameObject[gameManager.MainPlayer.InventorySize];
            SlotAmountComps = new Text[gameManager.MainPlayer.InventorySize];

            GameObject slot1 = InvObject.transform.Find("Slot1").gameObject;
            Slots[0] = slot1;
            SlotBgs[0] = slot1.GetComponent<Image>();
            SlotImages[0] = slot1.transform.Find("ItemSprite").gameObject;
            SlotImageComps[0] = SlotImages[0].GetComponent<Image>();
            SlotAmounts[0] = slot1.transform.Find("AmountText").gameObject;
            SlotAmountComps[0] = SlotAmounts[0].GetComponent<Text>();

            for (int i = 1; i < gameManager.MainPlayer.InventorySize; i++)
            {
                GameObject slot = Instantiate(slot1, InvObject.transform);
                RectTransform rect = slot.GetComponent<RectTransform>();
                Image bg = slot.GetComponent<Image>();
                GameObject image = slot.transform.Find("ItemSprite").gameObject;
                Image comp = image.GetComponent<Image>();
                GameObject amount = slot.transform.Find("AmountText").gameObject;
                Text text = amount.GetComponent<Text>();
                Slots[i] = slot;
                SlotRects[i] = rect;
                SlotBgs[i] = bg;
                SlotImages[i] = image;
                SlotImageComps[i] = comp;
                SlotAmounts[i] = amount;
                SlotAmountComps[i] = text;
                slot.name = "Slot" + (i + 1);
                rect.anchoredPosition = new Vector3(x: 55f * i + 5f, y: -5f, z: 0);
            }
        }

        public void UpdateView()
        {
            UpdateSelection();
            for (int i = 0; i < gameManager.MainPlayer.InventorySize; i++)
            {
                UpdateSlot(i);
            }
        }

        public void UpdateSlot(int slot)
        {
            ItemStack item = gameManager.MainPlayer.inventory.Items[slot];
            if (item != null)
            {
                SlotImageComps[slot].enabled = true;
                SlotImageComps[slot].sprite = resourceStorage.ByMaterial(item.Material);
                SlotAmountComps[slot].text = item.Amount == 1 ? "" : item.Amount + "";
            }
            else
            {
                SlotImageComps[slot].enabled = false;
                SlotImageComps[slot].sprite = null;
                SlotAmountComps[slot].text = "";
            }
        }

        public void UpdateSelection()
        {
            for (int i = 0; i < gameManager.MainPlayer.InventorySize; i++)
            {
                var color = SlotBgs[i].color;
                if (gameManager.MainPlayer.CurrentSlot == i)
                {
                    color.a = 0.85f;
                }
                else
                {
                    color.a = 0.25f;
                }
                SlotBgs[i].color = color;
            }
        }

    }
}
