using UnityEngine;
using TMPro; // For TextMeshPro
using System.IO; // For File Handling

public class FeedbackManager : MonoBehaviour
{
    public TMP_InputField feedbackInputField; // Reference to the input field
    private string filePath; // Path to the file

    void Start()
    {
        // Define the file path.
        // Typically is "C:\Users\<YourUsername>\AppData\LocalLow\<YourCompanyName>\<YourGameName>\Feedback.txt" on Windows
        filePath = Path.Combine(Application.persistentDataPath, "Feedback.txt");
        Debug.Log(filePath);
        //filePath = Directory.GetCurrentDirectory() + "\\Assets\\Feedback.txt"; // Project Pane

        // SAVE PLAYER DATA
        // Create the file if not exists
        if (!File.Exists(filePath))
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                Debug.Log("File created successfully at: " + filePath);
            }
        }

        string playInfo = $"PlayerID: {PlayerPrefs.GetInt("PlayerID")} | Time of day played: {Main.S.timePlayed} | Duration: {Main.S.durationPlayed} secs | Score: {Main.S.currentScore}\n";
        File.AppendAllText(filePath, playInfo);
    }

    // Button Click Event
    public void SubmitFeedback()
    {
        string feedback = feedbackInputField.text;

        if (string.IsNullOrEmpty(feedback))
        {
            Debug.Log("Feedback is empty!");
        }
        else
        {
            // Save the feedback to the file
            SaveFeedbackToFile(feedback);

            // Clear the input field after submission
            feedbackInputField.text = "";
        }
    }

    private void SaveFeedbackToFile(string feedback)
    {
        try
        {
            // Ensure the file exists or create it
            if (!File.Exists(filePath))
            {
                // Create the file if not exists
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    Debug.Log("File created successfully at: " + filePath);
                } // File is automatically closed here
            }

            // Append the feedback with a timestamp
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string feedbackEntry = $"PlayerID: {PlayerPrefs.GetInt("PlayerID")} | Feedback: [{timestamp}] {feedback}\n";

            File.AppendAllText(filePath, feedbackEntry);
            Debug.Log("Feedback saved successfully!");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to save feedback: " + e.Message);
        }
    }
}