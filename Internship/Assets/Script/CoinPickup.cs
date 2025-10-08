using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            // Add coins to player's score
            ParametersScript.scoreValue += coinValue;

            

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
