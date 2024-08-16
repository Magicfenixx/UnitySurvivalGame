using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealthBarScript : MonoBehaviour
    {

        private float _volume;
        private GameObject _health;
        private float _maxScale;

        private void Awake()
        {
            _health = transform.Find("Health").gameObject;
            _maxScale = _health.transform.localScale.x;
        }

        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value > 1) value = 1;
                if (value < 0) value = 0;
                _volume = value;
                float newScale = _maxScale * _volume;
                _health.transform.localScale = new Vector2(newScale, _health.transform.localScale.y);
                _health.transform.localPosition = new Vector2(-_maxScale / 2 + newScale / 2, _health.transform.localPosition.y);
            }
        }

    }
}