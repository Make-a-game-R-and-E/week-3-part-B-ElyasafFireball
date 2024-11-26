using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFuelManager : MonoBehaviour
{
    [SerializeField] float maxFuel = 10f;
    [SerializeField] float currentFuel;
    [SerializeField] float fuelConsumptionRate = 1f; // 1 fuel per second
    [SerializeField] float sizeIncreasePerFuel = 0.1f;
    [SerializeField] Vector3 baseSize = Vector3.one;

    void Start()
    {
        currentFuel = maxFuel;
    }

    void Update()
    {
        // Decrease fuel over time
        currentFuel -= fuelConsumptionRate * Time.deltaTime;
        currentFuel = Mathf.Clamp(currentFuel, 0f, Mathf.Infinity);

        // Adjust player size based on fuel
        AdjustSize();

        // Check if fuel is depleted
        if (currentFuel <= 0f)
        {
            // Handle game over state
            GameOver();
        }
    }

    // Method to add fuel when collecting fuel orbs
    public void AddFuel(float amount)
    {
        currentFuel += amount;
    }

    // Adjusts the player's size based on current fuel
    private void AdjustSize()
    {
        float newSize = 1f + (currentFuel - maxFuel) * sizeIncreasePerFuel;
        newSize = Mathf.Max(newSize, 0.1f); // Minimum size to prevent disappearing
        transform.localScale = baseSize * newSize;
    }

    // Handles the game over state
    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int getCurrentFuel()
    {
        return Mathf.RoundToInt(currentFuel);
    }
}
