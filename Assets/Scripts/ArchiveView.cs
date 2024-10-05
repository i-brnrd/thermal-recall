using UnityEngine;
using TMPro;
using UnityEngine.UI; // For using Button component
using System.IO;

public class ArchiveView : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab for each button element in the Scroll View
    public Transform contentPanel; // The Content of the Scroll View

    void Start()
    {
        PopulateScrollView();
    }

    void PopulateScrollView()
    {
        // Get all file names from the persistent data path
        string path = Application.persistentDataPath;
        string[] files = Directory.GetFiles(path);

        // Loop through all the files and create a new Button prefab for each one
        foreach (string filePath in files)
        {
            // Instantiate a new Button prefab
            GameObject newButton = Instantiate(buttonPrefab, contentPanel);

            // Get the file name from the full path and assign it to the TextMeshProUGUI component inside the button
            string fileName = Path.GetFileName(filePath);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = fileName;

            // Add an onClick listener to the button to handle file selection
            Button btnComponent = newButton.GetComponent<Button>();
            btnComponent.onClick.AddListener(() => OnFileSelected(filePath));
        }
    }

    // Method to handle what happens when a file is selected
    void OnFileSelected(string filePath)
    {
        Debug.Log("Selected file: " + filePath);
        // You can add further functionality here, like opening or processing the file.
    }
}
