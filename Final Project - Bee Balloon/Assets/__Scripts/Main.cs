using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;   // Enables the loading & reloading of scenes
using UnityEngine.UI; // For Legacy Text
using TMPro; // For TextMeshPro

public class Main : MonoBehaviour
{
    static public Main S;                        // A public singleton for Main
    private BoundsCheck bndCheck;

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
    public AudioClip onSuccessClip; // Audio to play on successful crate destruction
    public GameObject onDeathParticles; // Particle system prefabs to instantiate on crate destruction

    // ****** REMOVE *****
    public TextMeshProUGUI uitHighScore;
    public TextMeshProUGUI uitSBullets;
    public TextMeshProUGUI uitDBullets;
    public TextMeshProUGUI uitFBullets;
    public TextMeshProUGUI uitCrates;
    public GameObject[] prefabCrates;               // Array of Crate prefabs 
    public float crateSpawnPerSecond = 0.5f;  // # Crates spawned/second
    public float crateInsetDefault = 1.5f;    // Inset from the sides

    [Header("Dynamic")]
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

    // Level Related Attributes
    public int[] maxBalloons; // Max number of balloons per level.

    // ****** REMOVE *******
    public int highScore;
    public int numCurrSpawnedCrates;
    public int remainingCrates; // Number of crates left to destroy.
    public int numDestroyedCrates; // TOTAL crates destroyed in this level
    public int numCorrectlyDestroyedCrates; // For keeping track of the goal conditions. // A crate is correctly destroyed if its crate value reaches exactly 0.
    public int pointsPerSBullet = 1; // Strength of bullets from input "S"
    public int pointsPerDBullet = 2;
    public int pointsPerFBullet = 3;
    public int[] maxCrates; // Max number of crates to spawn per level
    public int[] maxBulletsPerLevel;
    public int[] remainingSBullets; // Number of "S" shots remaining, per level
    public int[] remainingDBullets;
    public int[] remainingFBullets;
    public int[] goals; // Set the goals for each level. Goals are based on number of correctly destroyed crates.

    void Awake()
    {
        S = this; // Define the singleton

        // Level Design.
        maxBalloons = new int[] { 68 };

        level = 0;
        levelMax = 1;
        maxBees = 3;
        numBees = maxBees;
        currentScore = 0;
        remainingBalloons = maxBalloons[level];
        numDestroyedBalloons = 0;

        startTime = Time.time;
        audioSource = GetComponent<AudioSource>();
        levelChangeText.enabled = false;

        //remainingCrates = maxCrates[level];
        //numDestroyedCrates = 0;
        //numCorrectlyDestroyedCrates = 0;
        
        /* // Level design
        maxBulletsPerLevel = new int[] { 50, 30, 20 };
        remainingSBullets = new int[] { maxBulletsPerLevel[0], maxBulletsPerLevel[1], maxBulletsPerLevel[2] };
        remainingDBullets = new int[] { maxBulletsPerLevel[0], maxBulletsPerLevel[1], maxBulletsPerLevel[2] };
        remainingFBullets = new int[] { maxBulletsPerLevel[0], maxBulletsPerLevel[1], maxBulletsPerLevel[2] };
        maxCrates = new int[] { 9, 12, 15 };
        goals = new int[] { 5, 7, 8 }; // Destroy more than half of the crates to reach this goal*/

        // Playerprefs for highscore
        /*if (PlayerPrefs.HasKey("HighScore"))
        {
            SCORE = PlayerPrefs.GetInt("HighScore");
        }
        // Assign the high score to HighScore
        PlayerPrefs.SetInt("HighScore", SCORE);*/


        // Set bndCheck to reference the BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();

        // Invoke SpawnCrate() once (in 2 seconds, based on default values)
        //Invoke(nameof(SpawnCrate), 2f);                // a
    }

    /*public void SpawnCrate()
    {
        // Pick a random Crate prefab to instantiate
        int ndx = Random.Range(0, prefabCrates.Length);                     // b
        GameObject go = Instantiate<GameObject>(prefabCrates[ndx]);     // c

        // Position the Crate above the screen with a random x position
        float crateInset = crateInsetDefault;                                // d
        if (go.GetComponent<BoundsCheck>() != null)
        {                        // e
            crateInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        // Set the initial position for the spawned Crate                    // f
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + crateInset;
        float xMax = bndCheck.camWidth - crateInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + crateInset;
        go.transform.position = pos;

        numCurrSpawnedCrates++; // Count spawned crate

        // Invoke SpawnCrate() again
        if (numCurrSpawnedCrates < maxCrates[level])
        {
            // Invoke SpawnCrate() in specified time
            Invoke(nameof(SpawnCrate), 1f / crateSpawnPerSecond);                // g
        }
    }*/

    // For showing the data in the GUITexts
    void UpdateGUI()
    {
        uitScore.text = "Score: " + currentScore.ToString();
        uitLevel.text = "Level: " + (level+1).ToString() + " of " + levelMax.ToString();
        uitBees.text = "Bees Left: " + numBees.ToString();

        //uitSBullets.text = "S (1): " + remainingSBullets[level].ToString();
        //uitDBullets.text = "D (2): " + remainingDBullets[level].ToString();
        //uitFBullets.text = "F (3): " + remainingFBullets[level].ToString();
        //uitCrates.text = "Crates Left: " + remainingCrates.ToString();

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

        // Check for level completion
        if (numDestroyedBalloons == maxBalloons[level])
        {
            // Check for non-max level.
            if ((level + 1) < levelMax)
            {
                level++;
                numBees = maxBees;
                numDestroyedBalloons = 0; 
                remainingBalloons = maxBalloons[level];
                bee.SetActive(true);

                // ****** INSTANTIATE A LEVEL PREFAB


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
                numBees = maxBees;
                numDestroyedBalloons = 0; 
                remainingBalloons = maxBalloons[level];
                bee.SetActive(true);

                // ****** INSTANTIATE A LEVEL PREFAB


                // Level up pop up sound and graphics
                audioSource.PlayOneShot(levelDownClip);
                levelChangeText.enabled = true;
                levelChangeText.text = "You lost a level!\nNow Level " + (level + 1).ToString() + "!";
                Invoke(nameof(HideLevelChangeGraphic), 2f); // Hide after 2 seconds

                // Invoke SpawnCrate() in specified time
                //Invoke(nameof(SpawnCrate), 1f / crateSpawnPerSecond);
            }
            // Level too low, game over.
            else
            {
                // call gameover screen
                finalMessage = "You used up all your lives. Try again!";
                SceneManager.LoadScene("GameOverScreen");
            }
        }
    }

    public void RespawnBee()
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