using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject Words;
    public GameObject Numbers;
    public GameObject FX;
    public Color col1;
    public Color col2;

    private GameData gameData;
    private TMP_Text word;
    private TMP_Text number;

    private float wordDelay = 2.0f;
    private float listDelay = 5.0f;
    private float numberShowDuration = 30.0f;
    private float promptDuration = 90.0f;

    private void Awake()
    {
        gameData = GetComponent<GameData>();
        word = Words.GetComponentInChildren<TMP_Text>();
        number = Numbers.GetComponentInChildren<TMP_Text>();


        FX.SetActive(true);
        Words.SetActive(false);
        Numbers.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Task());
    }

    private IEnumerator Task()

    {
        yield return new WaitForSeconds(0.2f); // can remove just for sanity

        yield return StartCoroutine(ShowFX(listDelay)); 
        yield return StartCoroutine(ShowList(gameData.list1, col1));
        yield return StartCoroutine(ShowFX(listDelay));
        yield return StartCoroutine(ShowList(gameData.list2, col2));
        yield return StartCoroutine(ShowFX(listDelay));
        yield return StartCoroutine(ShowNumber());

        yield return StartCoroutine(ShowPrompt("Recall Blue Words", col1));
        yield return StartCoroutine(ShowFX(2.0f));
        yield return StartCoroutine(ShowPrompt("Recall Green Words", col2));


    }  

    public IEnumerator ShowList(string[] wordlist, Color color)
    {
        int count = 0;
        string wordToShow;
       
        while (count < wordlist.Length)
        {
            wordToShow = wordlist[count];
            yield return StartCoroutine(ShowWord(wordToShow, color));
            yield return ShowFX(wordDelay);
            count++; 
        }
    }

    private IEnumerator ShowWord(string wordToShow, Color color)
    {
        float duration = Random.Range(2f, 3f);
        Words.SetActive(true);
        word.text = wordToShow;
        word.color = color;
        yield return new WaitForSeconds(duration);
        Words.SetActive(false);
    }

    private IEnumerator ShowFX(float delay)
    {
        FX.SetActive(true);
        yield return new WaitForSeconds(delay);
        FX.SetActive(false);
    }

    private IEnumerator ShowNumber()
    {
        int ran3dig = Random.Range(100, 1000);
        Numbers.SetActive(true);
        number.text = ran3dig.ToString();
        yield return new WaitForSeconds(numberShowDuration);
        Numbers.SetActive(false);
    }

    private IEnumerator ShowPrompt(string wordToShow, Color color)
    {
        Words.SetActive(true);
        word.text = wordToShow;
        word.color = color;
        yield return new WaitForSeconds(promptDuration);
        Words.SetActive(false);
    }


}
