using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] enum OrbType { Fuel, Cold, Oxygen }
    [SerializeField] OrbType orbType;
    [SerializeField] float lifetime = 10f;

    private OrbSpawner orbSpawner;

    void Start()
    {
        orbSpawner = FindObjectOfType<OrbSpawner>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerFuelManager playerFuelManager = collision.GetComponent<PlayerFuelManager>();
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerFuelManager != null && playerMovement != null)
            {
                switch (orbType)
                {
                    case OrbType.Fuel:
                        playerFuelManager.AddFuel(2f); // Add 2 seconds of fuel
                        orbSpawner.FuelOrbCollected(); // Notify orb spawner
                        break;
                    case OrbType.Cold:
                        playerMovement.ApplySpeedModifier(0.7f, 5f); // Slow down by 30% for 5 seconds
                        break;
                    case OrbType.Oxygen:
                        playerMovement.ApplySpeedModifier(1.5f, 3f); // Speed up by 50% for 3 seconds
                        break;
                }
            }
            // Destroy the orb after collection
            Destroy(gameObject);
        }
    }
}
