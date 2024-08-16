using Assets.Scripts;
using Assets.Scripts.Entities;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameManager gameManager;
    private EntityContainer entityContainer;
    private System.Random random;

    private Level level;
    private Level[] levels;
    private float currentSpawn;

    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            int lvl = value / 5;
            if (lvl > 5) return;
            level = levels[lvl];
        }
    }

    public EntityPlayer player;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        entityContainer = GameObject.Find("EntityContainer").GetComponent<EntityContainer>();

        random = new System.Random();

        levels = new Level[6];
        levels[0] = new Level()
        {
            minCount = 1,
            maxCount = 3,
            types = new EntityType[] { EntityType.ZOMBIE },
            minNextSpawn = 15,
            maxNextSpawn = 25
        };
        levels[1] = new Level()
        {
            minCount = 1,
            maxCount = 4,
            types = new EntityType[] { EntityType.ZOMBIE },
            minNextSpawn = 12,
            maxNextSpawn = 21
        };
        levels[2] = new Level()
        {
            minCount = 2,
            maxCount = 4,
            types = new EntityType[] { EntityType.ZOMBIE, EntityType.ZOMBIE, EntityType.SKELETON },
            minNextSpawn = 13,
            maxNextSpawn = 20
        };
        levels[3] = new Level()
        {
            minCount = 3,
            maxCount = 5,
            types = new EntityType[] { EntityType.ZOMBIE, EntityType.SKELETON, EntityType.SKELETON },
            minNextSpawn = 13,
            maxNextSpawn = 18
        };
        levels[4] = new Level()
        {
            minCount = 4,
            maxCount = 6,
            types = new EntityType[] { EntityType.ZOMBIE, EntityType.SKELETON, EntityType.SKELETON },
            minNextSpawn = 11,
            maxNextSpawn = 16
        };
        levels[5] = new Level()
        {
            minCount = 1,
            maxCount = 1,
            types = new EntityType[] { EntityType.CAT },
            minNextSpawn = 35,
            maxNextSpawn = 35
        };
        level = levels[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || player.IsDestroyed()) return;
        if (currentSpawn <= 0)
        {
            int spawnCount = random.Next(level.minCount, level.maxCount + 1);
            for (int i = 0; i < spawnCount; i++)
            {
                float originX = player.transform.position.x;
                float x = originX + (random.Next(70) - 35);
                EntityType type = level.types[random.Next(level.types.Length)];
                Entity ent = entityContainer.CreateEntity(type, x, 10);
                if(ent is EntityZombie)
                {
                    ((EntityZombie)ent).Target = player;
                }
                if (ent is EntitySkeleton)
                {
                    ((EntitySkeleton)ent).Target = player;
                }
                if (ent is EntityCat)
                {
                    ((EntityCat)ent).Target = player;
                }

            }
            currentSpawn = random.Next(level.minNextSpawn, level.maxNextSpawn + 1);
        }
        else
        {
            currentSpawn -= Time.deltaTime;
        }
    }

    class Level
    {

        internal int minCount;
        internal int maxCount;
        internal EntityType[] types;
        internal int minNextSpawn;
        internal int maxNextSpawn;

    }

}
