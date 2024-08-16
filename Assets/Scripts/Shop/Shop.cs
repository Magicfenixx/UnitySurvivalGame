using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    private GameObject goldCount;
    private Text goldCountText;

    private void Awake()
    {
        goldCount = transform.Find("GoldCount").gameObject;
        goldCountText = goldCount.GetComponent<Text>();
    }

    public int GoldCount
    {
        get
        {
            return int.Parse(goldCountText.text);
        }
        set
        {
            goldCountText.text = value.ToString();
        }
    }


}
