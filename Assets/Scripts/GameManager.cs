using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject Words;
    public GameObject Numbers;


    private GameData gameData;
    // Awake

    private void Awake()
    {
        gameData = GetComponent<GameData>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowWords();
    }


    private void ShowWords()
    {
        Debug.Log("in show words");

        Debug.Log(gameData.list1);
        Debug.Log(gameData.list2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
