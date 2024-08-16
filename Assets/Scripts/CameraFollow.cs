using Assets.Scripts.Blocks;
using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        
        [HideInInspector] public GameManager gameManager;
        [HideInInspector] public GameObject background;
            private ResourceStorage resourceStorage;


        void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            background = GameObject.Find("Background");
        }

        void LateUpdate()
        {
            if (gameManager.MainPlayer == null) return;

            float x = gameManager.MainPlayer.transform.position.x;
            float y = gameManager.MainPlayer.transform.position.y;
            transform.position = new Vector3(x, y, -10);
            background.transform.position = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);
        }
    }
}