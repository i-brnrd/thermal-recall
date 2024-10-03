using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Color col1;
    public Color col2;

    public float presentationFontSize = 350.0f;
    public float promptFontSize = 150.0f;

    public float wordDisplayInterval = 2.0f;
    public float listTransitionDelay = 4.0f;
    public float numberDisplayDuration = 30.0f;
    public float recallPromptDuration = 90.0f;

    public GameObject StartScreen;
    public GameObject Words;
    public GameObject Numbers;
    public GameObject FX;

    private GameData gameData;
    private EmailRecordings emailRecordings;
    private TMP_Text word;
    private TMP_Text number;

    private string rec1path;
    private string rec2path;

    private void Awake()
    {
        gameData = GetComponent<GameData>();
        emailRecordings = GetComponent<EmailRecordings>();
        rec1path = Application.persistentDataPath + "/rec1.wav";
        rec2path = Application.persistentDataPath + "/rec2.wav";
        word = Words.GetComponentInChildren<TMP_Text>();
        number = Numbers.GetComponentInChildren<TMP_Text>();

        FX.SetActive(false);
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

        yield return StartCoroutine(ShowPrompt("Learn new blue list", Color.white, listTransitionDelay));
        yield return ShowFX(wordDisplayInterval);
        yield return StartCoroutine(ShowList(gameData.list1, col1));
        yield return StartCoroutine(ShowPrompt("Learn new green list", Color.white, listTransitionDelay));
        yield return ShowFX(wordDisplayInterval);
        yield return StartCoroutine(ShowList(gameData.list2, col2));
        yield return StartCoroutine(ShowPrompt("Count back in 3s from...", Color.white, listTransitionDelay));
        yield return StartCoroutine(ShowNumber());
        yield return StartCoroutine(ShowRecallPrompt("Recall Blue Words", col1, rec1path));
       
        yield return StartCoroutine(ShowFX(1.0f));
        yield return StartCoroutine(ShowRecallPrompt("Recall Green Words", col2, rec2path));
#if !UNITY_WEBGL  // Don't email if WebGL; won't work 
        yield return StartCoroutine(ShowPrompt("End of task, emailing results", Color.white, listTransitionDelay));
        emailRecordings.SendEmails(rec1path);
        emailRecordings.SendEmails(rec2path);
#endif
#if UNITY_WEBGL
        yield return StartCoroutine(ShowPrompt("End of task", Color.white, 10.0f));
#endif
    }

    public IEnumerator ShowList(string[] wordlist, Color color)
    {
        foreach(var word in wordlist)
        {
            yield return StartCoroutine(ShowWord(word, color));
            yield return ShowFX(wordDisplayInterval);
        }
    }

    private IEnumerator ShowWord(string wordToShow, Color color)
    {
        Words.SetActive(true);
        word.text = wordToShow;
        word.color = color;
        word.fontSize = presentationFontSize;
        yield return new WaitForSeconds(Random.Range(2f, 3f));
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
        number.fontSize = presentationFontSize;
        yield return new WaitForSeconds(numberDisplayDuration);
        Numbers.SetActive(false);
    }

    private IEnumerator ShowPrompt(string wordToShow, Color color, float duration)
    {
        Words.SetActive(true);
        word.text = wordToShow;
        word.color = color;
        word.fontSize = promptFontSize;
        yield return new WaitForSeconds(duration);
        Words.SetActive(false);
    }

    private IEnumerator ShowRecallPrompt(string wordToShow, Color color, string path)
    {
        Words.SetActive(true);
        word.text = wordToShow;
        word.color = color;
        word.fontSize = promptFontSize;

#if !UNITY_WEBGL  // Exclude microphone recording in WebGL builds
        // Start recording from the microphone
        AudioClip recordedClip = Microphone.Start(null, false, (int)recallPromptDuration, 44100);
    Debug.Log("Recording started");
#endif

        yield return new WaitForSeconds(recallPromptDuration);

#if !UNITY_WEBGL  // Exclude microphone recording in WebGL builds
    // Stop recording
    Microphone.End(null);
    Debug.Log("Recording stopped");

        SavWav.Save(path, recordedClip);
   // WavUtility.Save(recordedClip, path);
#endif
       
        Words.SetActive(false);
    }

}
