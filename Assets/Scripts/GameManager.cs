using Assets.Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {

        private EntityContainer entityContainer;
        private PlayerInterface playerInterface;
        private EnemySpawner enemySpawner;
        private GameObject menuPanel;

        [HideInInspector] public EntityPlayer MainPlayer;

        private void Awake()
        {
            entityContainer = GameObject.Find("EntityContainer").GetComponent<EntityContainer>();
            playerInterface = GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>();
            enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
            menuPanel = GameObject.Find("PlayerInterface").transform.Find("MenuPanel").gameObject;
        }

        public void OpenMenuPanel()
        {
            menuPanel.SetActive(true);
        }

        public void CloseMenuPanel()
        {
            menuPanel.SetActive(false);
        }

        public void ToggleMenuPanel()
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }

        private void Start()
        {
            EntityPlayer player = (EntityPlayer)entityContainer.CreateEntity(EntityType.PLAYER, 0, 10);
            MainPlayer = player;
            playerInterface.Initialize();

            enemySpawner.player = player;
            //EntityZombie zombie = (EntityZombie)entityContainer.CreateEntity(EntityType.ZOMBIE, -5, 10);
            //zombie.Target = player;
            //EntityCat cat = (EntityCat)entityContainer.CreateEntity(EntityType.CAT, -10, 10);
            //cat.Target = player;
        }

    }
}

