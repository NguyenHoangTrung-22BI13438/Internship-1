using UnityEngine;
using TMPro;

public class ParametersScript : MonoBehaviour
{
    public static int scoreValue = 0;    // coins
    public static int healValue = 1000; // HP
    public TextMeshProUGUI parameters;

    void Start()
    {
        healValue = 1000;
    }

    void Update()
    {
        parameters.text = $"COINS: {scoreValue}\nHP: {healValue}";
    }
}
