using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LatestLevel : MonoBehaviour
{
    TextMeshProUGUI uitLastLevel;
    string defaultLevel = "None";

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the last level or use a default message if none is found
        string lastLevel = PlayerPrefs.GetString("LastLevel", defaultLevel);

        uitLastLevel = GetComponent<TextMeshProUGUI>();
        uitLastLevel.text = "Last Played Level: " + lastLevel;
    }
}
