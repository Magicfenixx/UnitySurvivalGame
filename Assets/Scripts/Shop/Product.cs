using Assets.Scripts;
using Assets.Scripts.Entities;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Product : MonoBehaviour
{

    private GameManager gameManager;
    private EntityContainer entityContainer;

    private Image image;
    private Text priceText;

    public ProductType Type;
    public int Price;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        entityContainer = GameObject.Find("EntityContainer").GetComponent<EntityContainer>();

        image = GetComponent<Image>();
        priceText = transform.Find("PriceText").gameObject.GetComponent<Text>();
        priceText.text = Price.ToString();
    }

    public void ProductClick()
    {
        EntityPlayer player = gameManager.MainPlayer;
        int goldCount = player.inventory.AmountOf(Assets.Scripts.Material.GOLD);
        if(goldCount >= Price) {
            player.inventory.TakeItems(Assets.Scripts.Material.GOLD, Price);
            Purchase(player);
        } else
        {
            Debug.Log("Not enough gold");
        }
    }

    public void Purchase(EntityPlayer player)
    {
        switch (Type)
        {
            case ProductType.Health:
                {
                    player.Health += 5;
                    player.healthScript.Volume = player.Health / player.MaxHealth;
                    break;
                }
            case ProductType.Gun:
                {
                    bool result = player.inventory.AddItem(Assets.Scripts.Material.GUN, 1);
                    if (!result) entityContainer.DropItem(player.transform.position.x, player.transform.position.y, new Assets.Scripts.Items.ItemStack(Assets.Scripts.Material.GUN, 1));
                    break;
                }
            case ProductType.Speed:
                {
                    player.HorizontalSpeed += 3;
                    break;
                }
            case ProductType.Bomb:
                {
                    bool result = player.inventory.AddItem(Assets.Scripts.Material.BOMB, 1);
                    if (!result) entityContainer.DropItem(player.transform.position.x, player.transform.position.y, new Assets.Scripts.Items.ItemStack(Assets.Scripts.Material.BOMB, 1));
                    break;
                }
            case ProductType.Ammo:
                {
                    bool result = player.inventory.AddItem(Assets.Scripts.Material.AMMO, 5);
                    if (!result) entityContainer.DropItem(player.transform.position.x, player.transform.position.y, new Assets.Scripts.Items.ItemStack(Assets.Scripts.Material.AMMO, 5));
                    break;
                }
        }
    }

    public void ProductEnter()
    {
        Color color = image.color;
        color.r -= 0.3f;
        color.g -= 0.3f;
        color.b -= 0.3f;
        image.color = color;
    }

    public void ProductExit()
    {
        Color color = image.color;
        color.r += 0.3f;
        color.g += 0.3f;
        color.b += 0.3f;
        image.color = color;
    }


}
