using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerFuelManager playerFuelManager;
    [SerializeField] TextMeshProUGUI fuelText;

    void Update()
    {
        if (playerFuelManager != null)
        {
            fuelText.text = "Fuel: " + Mathf.RoundToInt(playerFuelManager.getCurrentFuel()).ToString();
        }
    }
}
