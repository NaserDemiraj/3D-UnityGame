using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            GameController gameController = FindObjectOfType<GameController>(); // Find the GameController
            if (gameController != null) // Check if the GameController was found
            {   
                gameController.CollectItem(); // Notify GameController
                gameObject.SetActive(false); // Deactivate the item
            }
            else
            {
                Debug.LogWarning("GameController not found!"); // Warn if GameController is missing
            }
        }
    }
}
