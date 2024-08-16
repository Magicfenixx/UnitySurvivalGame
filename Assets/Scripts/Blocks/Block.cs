using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class Block : MonoBehaviour
    {

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider;
        private ResourceStorage resourceStorage;

        private Material _material;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();
        }

        public Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                _material = value;
                switch(value)
                {
                    case Material.AIR:
                        {
                            boxCollider.enabled = false;
                            spriteRenderer.enabled = true;
                            spriteRenderer.sprite = null;
                            break;
                        }
                    case Material.GRASS:
                        {
                            boxCollider.enabled = true;
                            spriteRenderer.enabled = true;
                            spriteRenderer.sprite = resourceStorage.GrassSprite;
                            break;
                        }
                    case Material.STONE:
                        {
                            boxCollider.enabled = true;
                            spriteRenderer.enabled = true;
                            spriteRenderer.sprite = resourceStorage.StoneSprite;
                            break;
                        }
                    case Material.GOLD:
                        {
                            boxCollider.enabled = true;
                            spriteRenderer.enabled = true;
                            spriteRenderer.sprite = resourceStorage.GoldSprite;
                            break;
                        }
                }
            }
        }

    }
}