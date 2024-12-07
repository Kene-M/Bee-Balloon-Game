/* TO-DO
 * Implement some player feedback boxes.
 * Save above and other data to file. - PlayerPrefs for the ID system???
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // Enables the loading & reloading of scenes
using UnityEngine.UI; // For Legacy Text
using TMPro;
using System.Runtime.CompilerServices;

public class Main : MonoBehaviour
{
    static public Main S;                        // A public singleton for Main
    //private BoundsCheck bndCheck;

    [Header("Inscribed")]
    public TextMeshProUGUI uitScore;
    public TextMeshProUGUI uitLevel;
    public TextMeshProUGUI uitBees;  
    public TextMeshProUGUI uitCountdown;
    public TextMeshProUGUI levelChangeText;
    public Button spawnButton;

    public GameObject bee; // CHANGE THIS WHEN MORE LEVELS ARE ADDED

    public GameObject[] prefabLevels; // Array of level prefabs
    public AudioClip levelUpClip;
    public AudioClip levelDownClip;
    //public AudioClip onSuccessClip; // Audio to play on successful crate destruction
    public GameObject onDeathParticles; // Particle system prefabs to instantiate on bee destruction
    public GameObject bombParticles;
    public GameObject redZoneParticles;

    [Header("Dynamic")]
    GameObject levelGameObj; // An instantiated prefab of the current level
    public int currentScore;
    public int level;
    public int levelMax;
    public int numBees; // Lives Left / Current bees
    public int maxBees; // max bees.
    public int remainingBalloons; // Number of balloons currently left to destroy.
    public int numDestroyedBalloons; // TOTAL crates destroyed in this level
    public int pointsPerBalloon = 2; // Points per balloon pop

    private AudioSource audioSource; // Audio on level up
    private float startTime; // For countdown.
    private int timeRemain; // For countdown.
    public string finalMessage; // Message to display at end scene.

    // PlayerPrefs
    public int highScore; // Not implemented
    public string lastLevel; // Last Level Played
    public int playerID;

    // To save to file
    public string timePlayed;
    public int durationPlayed;

    // Level Related Attributes
    public int[] maxBalloons; // Max number of balloons per level.
    Vector2[] spawnPositions; // The inital/reset positions of the respawn button.

    void Awake()
    {
        S = this; // Define the singleton

        // Initial level design.
        maxBalloons = new int[] { 68, 53, 32, 80 };
        spawnPositions = new Vector2[] { new Vector2(1.1f, 2.9f), new Vector2(-253f, -3f), new Vector2(9.8f, 36.4f), new Vector2(-317f, 116f) };
        level = 0;
        SetLatestLevel((level + 1).ToString()); // PlayerPrefs
        levelMax = prefabLevels.Length;
        maxBees = 3;
        numBees = maxBees;
        currentScore = 0;
        remainingBalloons = maxBalloons[level];
        numDestroyedBalloons = 0;

        startTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        levelChangeText.enabled = false;

        // Level instance setup
        levelGameObj = Instantiate<GameObject>(prefabLevels[level]); // Instantiate the first level at the beginning.
        bee = levelGameObj.GetComponentInChildren<BeeTest>().gameObject; // get the bee child gameobject
        //bee.transform.position = new Vector3(0f, -0.5f, -1.12f);
        spawnButton.GetComponent<RectTransform>().anchoredPosition = spawnPositions[level]; // spawn button setup
        bee.SetActive(false);

        // File Save
        timePlayed = System.DateTime.Now.ToString();
        
        // PlayerPrefs for lastLevel.
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            LAST_LEVEL = PlayerPrefs.GetString("LastLevel");
        }
        // Assign the latest level to lastLevel
        PlayerPrefs.SetString("LastLevel", LAST_LEVEL);

        // PlayerPrefs for PlayerID.
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            Main.S.playerID = PlayerPrefs.GetInt("PlayerID");
        }
        else
        {
            Main.S.playerID = Random.Range(1000, 10000); // Generate a new 4 digit ID.
        }
        // Assign the playerID
        PlayerPrefs.SetInt("PlayerID", Main.S.playerID);

        // Playerprefs for highscore
        /*if (PlayerPrefs.HasKey("HighScore"))
        {
            SCORE = PlayerPrefs.GetInt("HighScore");
        }
        // Assign the high score to HighScore
        PlayerPrefs.SetInt("HighScore", SCORE);*/
    }

    // For showing the data in the GUITexts
    void UpdateGUI()
    {
        uitScore.text = "Score: " + currentScore.ToString();
        uitLevel.text = "Level: " + (level + 1).ToString() + " of " + levelMax.ToString();
        uitBees.text = "Bees Left: " + numBees.ToString();

        // Countdown logic
        int timeElapsed = (int)(Time.time - startTime);
        //print(timeElapsed);
        timeRemain = 240 - timeElapsed; // 4 minute countdown

        if (timeRemain > 0)
        {
            uitCountdown.text = "Time Remaining: " + timeRemain.ToString();
        }
        else
        {
            uitCountdown.text = "Countdown has finished";

            // Other conditions to end game: Timer runs out.
            durationPlayed = timeElapsed; // For file saving

            finalMessage = "You ran out of time. Try again!";
            SceneManager.LoadScene("GameOverScreen");
        }

        Color c = uitCountdown.color;
        c.a = timeRemain / 240f;
        uitCountdown.color = c;
    }

    void Update()
    {
        UpdateGUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.S.Pause();
        }

        // Check for level completion
        if (numDestroyedBalloons == maxBalloons[level])
        {
            // Check for non-max level.
            if ((level + 1) < levelMax)
            {
                level++;
                SetLatestLevel((level + 1).ToString()); // PlayerPrefs
                numBees = maxBees;
                numDestroyedBalloons = 0;
                remainingBalloons = maxBalloons[level];

                // INSTANTIATE A LEVEL PREFAB
                Destroy(levelGameObj);
                levelGameObj = Instantiate<GameObject>(prefabLevels[level]);
                bee = levelGameObj.GetComponentInChildren<BeeTest>().gameObject; // get the bee child gameobject
                spawnButton.GetComponent<RectTransform>().anchoredPosition = spawnPositions[level]; // spawn button setup
                spawnButton.gameObject.SetActive(true);
                bee.SetActive(false);

                //if (level == 2) // Check if level 3
                //{
                //    levelGameObj.GetComponentInChildren<Level3Zone1>().SetCenter(bee.transform.position); // set the center to be the bee's position.
                //}

                // Level up pop up sound and graphics
                audioSource.PlayOneShot(levelUpClip);
                levelChangeText.enabled = true;
                levelChangeText.text = "Level Up!\nReached Level " + (level + 1).ToString() + "!";
                Invoke(nameof(HideLevelChangeGraphic), 2f); // Hide after 2 seconds
            }
            // Beat the game.
            else
            {
                // call gameover screen 
                int timeElapsed = (int)(Time.time - startTime);
                durationPlayed = timeElapsed; // File save

                finalMessage = "Congrats, you beat the game!\n" + timeRemain.ToString() + " leftover secs have been added your final score.";
                currentScore += timeRemain;
                //TRY_TO_SET_HIGH_SCORE(currentScore);
                SceneManager.LoadScene("GameOverScreen");
            }
        }
        // Check for level down [other conditions include time runs out]
        else if (numBees == 0)
        {
            // Check for non-min level.
            if (level > 0)
            {
                level--;
                SetLatestLevel((level + 1).ToString()); // PlayerPrefs
                numBees = maxBees;
                numDestroyedBalloons = 0;
                remainingBalloons = maxBalloons[level];

                // INSTANTIATE A LEVEL PREFAB
                Destroy(levelGameObj);
                levelGameObj = Instantiate<GameObject>(prefabLevels[level]);
                bee = levelGameObj.GetComponentInChildren<BeeTest>().gameObject; // get the bee child gameobject
                spawnButton.GetComponent<RectTransform>().anchoredPosition = spawnPositions[level]; // spawn button setup
                spawnButton.gameObject.SetActive(true);
                bee.SetActive(false);

                if (level == 2) // Check if level 3
                {
                    levelGameObj.GetComponentInChildren<Level3Zone1>().SetCenter(bee.transform.position); // set the center to be the bee's position.
                }

                // Level up pop up sound and graphics
                audioSource.PlayOneShot(levelDownClip);
                levelChangeText.enabled = true;
                levelChangeText.text = "You lost a level!\nNow Level " + (level + 1).ToString() + "!";
                Invoke(nameof(HideLevelChangeGraphic), 2f); // Hide after 2 seconds
            }
            // Level too low, game over.
            else
            {
                // call gameover screen
                int timeElapsed = (int)(Time.time - startTime);
                durationPlayed = timeElapsed; // File save

                finalMessage = "You used up all your lives. Try again!";
                SceneManager.LoadScene("GameOverScreen");
            }
        }
    }

    // Event handler for button click.
    public void SpawnBee()
    {
        bee.SetActive(true);
        spawnButton.gameObject.SetActive(false);
    }

    // Amount of points to add or deduct
    public void AwardPoints()
    {
        currentScore += pointsPerBalloon;

        // TRY_TO_SET_HIGH_SCORE(currentScore);
    }

    void HideLevelChangeGraphic()
    {
        levelChangeText.enabled = false;
    }

    public void SetLatestLevel(string levelToTry)
    {
        LAST_LEVEL = levelToTry;
    }

    public string LAST_LEVEL
    {
        get { return lastLevel; }
        private set
        {
            lastLevel = value;
            PlayerPrefs.SetString("LastLevel", value);
        }
    }

    /*public void TRY_TO_SET_HIGH_SCORE(int scoreToTry)
    {
        if (scoreToTry <= SCORE) return;
        SCORE = scoreToTry;
    }*/

    /*public int SCORE
    { 
        get { return highScore; } 
        private set 
        { 
            highScore = value;
            PlayerPrefs.SetInt("HighScore", value);
            if (uitHighScore != null)
            {
                uitHighScore.text = "High Score: " + value.ToString();            
            }
        }
    }*/
}