using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public int fruitCount;

    public int waterCount;
     public TextMeshProUGUI fruitText;

      public string fruitType = "Wheat";

       public Button wheatButton;
    public Button fruitButton;

    public static GameManager Instance;

    public bool isWateringAnim = false;

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

    public void CollectWater()
    {
        waterCount++;
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

    public bool WaterPlant()
    {
        if (waterCount > 0)
        {
            waterCount--;
            return true;
        }
        return false;
    }
    
}
