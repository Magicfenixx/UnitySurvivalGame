using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ResourceStorage : MonoBehaviour
    {

        public GameObject ChunkPrefab;
        public GameObject BlockPrefab;
        public GameObject ItemPrefab;
        public GameObject PlayerPrefab;
        public GameObject ZombiePrefab;
        public GameObject SkeletonPrefab;
        public GameObject CatPrefab;
        public GameObject InventoryPrefab;
        public GameObject HealthBarPrefab;
        public GameObject BombPrefab;
        public GameObject ExplosionParticlesPrefab;
        public GameObject CatBulletPrefab;

        public AudioClip JumpAudio;
        public AudioClip SparklingAudio;
        public AudioClip ExplosionAudio;
        public AudioClip ShootAudio;
        public AudioClip[] DamageSounds;
        public AudioClip AmbientAudio;
        public AudioClip DeathAudio;
        public AudioClip EmptyShot;

        public Sprite GrassSprite;
        public Sprite StoneSprite;
        public Sprite GoldSprite;
        public Sprite GunSprite;
        public Sprite BombSprite;
        public Sprite AmmoSprite;

        public Sprite ByMaterial(Material material)
        {
            switch (material)
            {
                case Material.AIR: return null;
                case Material.GRASS: return GrassSprite;
                case Material.STONE: return StoneSprite;
                case Material.GOLD: return GoldSprite;
                case Material.GUN: return GunSprite;
                case Material.BOMB: return BombSprite;
                case Material.AMMO: return AmmoSprite;
            }
            return null;
        }

    }

}
