using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainBeeScript : MonoBehaviour
{
    public GameObject BeePrefab;
    public Button spawnButton;

    

    public void SpawnPrefab()
    {
        if (BeePrefab != null)
        {
            BeePrefab.SetActive(true);
            spawnButton.gameObject.SetActive(false); 
        }
        else
        {
            Debug.LogError("Prefab not assigned");
        }
    }
}
