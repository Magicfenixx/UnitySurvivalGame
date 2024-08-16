using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityBomb : Entity
    {

        public float PrimeTime;
        public float ExplosionRadius;

        protected override void Awake()
        {
            base.Awake();
            AudioSource.PlayClipAtPoint(resourceStorage.SparklingAudio, transform.position);
        }

        protected override void Update()
        {
            base.Update();
            PrimeTime -= Time.deltaTime;
            if(PrimeTime <= 0) {
                Remove();
                entityContainer.MakeExplosion(transform.position, ExplosionRadius);
            }
        }

    }
}
