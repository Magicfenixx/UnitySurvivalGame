using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class EntityLiving : Entity
    {

        private GameObject healthObject;
        public HealthBarScript healthScript;
        private System.Random random;
        private float _health = 20;
        private float _maxHealth = 20;

        protected override void Awake()
        {
            base.Awake();
            healthObject = Instantiate(resourceStorage.HealthBarPrefab, transform);
            healthScript = healthObject.GetComponent<HealthBarScript>();
            random = new();
            healthScript.transform.localPosition = new Vector3(0, boxCollider.size.y, 0);
            healthScript.transform.localScale = new Vector3(1.6f, 0.3f, 1);
        }

        public float Health
        {
            get { return _health; }
            set
            {
                if (value <= 0)
                {
                    value = 0;
                    Death();
                }
                if (value > _maxHealth) value = _maxHealth;
                _health = value;
            }
        }

        protected virtual void Death()
        {
            Remove();
            ScoreManager.ScoreCount += 1;
            AudioSource.PlayClipAtPoint(resourceStorage.DeathAudio, transform.position, 20f);
        }

        public float MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                if (value <= 0) return;
                _maxHealth = value;
            }
        }

        public bool IsAlive()
        {
            return _health > 0;
        }

        public void Damage(float damage)
        {
            Damage(damage, null);
        }

        public void Damage(float damage, Entity damager)
        {
            Health -= damage;
            int i = random.Next(7);
            AudioSource.PlayClipAtPoint(resourceStorage.DamageSounds[i], transform.position, 20f);
            healthScript.Volume = _health / _maxHealth;
            if (damager != null) {
            
                rigid.velocity = new Vector2(damager.transform.position.x > transform.position.x ? -5 : 5, 0);
            }
        }

        private void OnMouseDown()
        {
            if(Vector3.Distance(gameManager.MainPlayer.transform.position, transform.position) <= 5)
            {
                Damage(1f, gameManager.MainPlayer);
            }
        }
    }
}