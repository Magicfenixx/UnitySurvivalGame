using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText;

    private static int _score;
    public static int ScoreCount
    {
        get {
            return _score;
        }
        set
        {
            _score = value;
            EnemySpawner.Score = value;
        }
    }

    public static EnemySpawner EnemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        EnemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text="Score: "+ Mathf.Round(ScoreCount) + "\nLevel: " + (ScoreCount / 5 + 1);
    }
}
