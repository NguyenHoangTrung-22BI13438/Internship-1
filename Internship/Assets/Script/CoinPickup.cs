using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip pickupSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add to coin total
            ParametersScript.scoreValue += coinValue;

            // Play sound
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
