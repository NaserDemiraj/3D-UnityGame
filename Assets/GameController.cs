using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public float gameTime = 50f; // Start time for the game
    public float minimumTime = 25f; // Minimum allowed game time
    public float timeReduction = 5f; // Time deducted for each level

    public TMP_Text timerText; // UI Text to display the timer
    public TMP_Text levelText; // UI Text to display the current level
    public GameObject winPanel; // Win message panel
    public GameObject losePanel; // Lose message panel
    public GameObject nextLevelButton; // Button to start the next level
    public GameObject[] items; // All collectible items in the scene
    public int activeItemsCount = 5; // Number of items to activate per round

    private float currentTime;
    private int itemsCollected = 0;
    private int currentLevel = 1; // Track the current level
    private bool levelComplete = false; // Flag to check if the level is completed
    private bool isTimerRunning = true; // Controls whether the timer updates

    void Start()
    {
        StartLevel(); // Initialize the first level
    }

    void Update()
    {
        // Countdown timer
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }
        else if (currentTime <= 0 && !losePanel.activeSelf)
        {
            EndGame(false); // Lose condition when timer reaches 0
        }

        // Listen for the spacebar press to start the next level
        if (nextLevelButton.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            OnNextLevelButtonPressed(); // Trigger the next level functionality
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        if (itemsCollected >= activeItemsCount)
        {
            CompleteLevel(); // Mark the level as completed
        }
    }

    void CompleteLevel()
    {
        levelComplete = true; // Mark the level as completed
        isTimerRunning = false; // Stop the timer
        nextLevelButton.SetActive(true); // Show "Next Level" button
        winPanel.SetActive(true); // Display win panel
    }

    public void OnNextLevelButtonPressed()
    {
        Time.timeScale = 1; // Resume the game
        isTimerRunning = true; // Restart the timer
        nextLevelButton.SetActive(false); // Hide the button
        NextLevel(); // Start the next level
    }

    void NextLevel()
    {
        currentLevel++; // Increment the level

        // Reduce time for the next level, ensuring it doesn't drop below the minimum
        gameTime = Mathf.Max(minimumTime, gameTime - timeReduction);

        StartLevel(); // Start the next level
    }

    void StartLevel()
    {
        // Reset the game state for the new level
        currentTime = gameTime;
        itemsCollected = 0;
        levelComplete = false;
        isTimerRunning = true; // Ensure the timer starts running

        // Hide win/lose panels
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        // Update the level UI text
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }

        // Deactivate all items and activate a new random set
        DeactivateAllItems();
        ActivateRandomItems();
    }

    public void EndGame(bool hasWon)
    {
        isTimerRunning = false; // Stop the timer
        Time.timeScale = 0; // Pause the game

        if (hasWon)
        {
            winPanel.SetActive(true);
            nextLevelButton.SetActive(true); // Show the button, but listen for spacebar
        }
        else
        {
            losePanel.SetActive(true); // Show lose panel
        }
    }

    public void DeactivateAllItems()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(false); // Deactivate all items
        }
    }

    public void ActivateRandomItems()
    {
        // Shuffle the items and activate the first `activeItemsCount` items
        List<GameObject> itemList = new List<GameObject>(items);
        itemList.Shuffle();

        for (int i = 0; i < activeItemsCount; i++)
        {
            itemList[i].SetActive(true);
        }
    }
}

// Extension method for shuffling the list (using Fisher-Yates algorithm)
public static class ListExtensions
{
    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
