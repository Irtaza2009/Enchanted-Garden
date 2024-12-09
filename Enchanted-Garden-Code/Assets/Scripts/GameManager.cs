using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int fruitCount;
     public TextMeshProUGUI fruitText;

      public string fruitType = "Wheat";

       public Button wheatButton;
    public Button fruitButton;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateUI();
    }
    public void CollectFruit()
    {
        fruitCount++;

    }

    void UpdateUI()
    {
         if (fruitText != null) fruitText.text = fruitCount.ToString();
    }

     public void FruitSwitch()
    {
        if (fruitType == "Wheat")
        {
            fruitType = "Fruit";
            wheatButton.interactable = true;
            fruitButton.interactable = false;
        }
        else
        {
            fruitType = "Wheat";
            wheatButton.interactable = false;  // Disable the button to indicate it's selected
            fruitButton.interactable = true;   // Enable the other button
        }
    }
    
}
