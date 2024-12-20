using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class GameManager : MonoBehaviour
{

    // TODO:
    // selling
    // menu scene
    // workers
    // add variety of fruits
    // add fairies flying randomly

    public Tilemap leftFenceTilemap; // Reference to the left fence tilemap
    public Tilemap topFenceTilemap;  // Reference to the top fence tilemap

    public Tilemap rightFenceTilemap; // Reference to the right fence tilemap
    public GameObject topFenceTilemapPrefab; // Prefab for fence tilemaps
    public GameObject hillTilemapPrefab; // Prefab for fence tilemaps

    public GameObject[] boundaries;

    public GameObject leftBoundary;

    public Transform Grid;

        public int landCost = 10;
        public int landSize = 1;
    public int oreCount;

    public int fruitCount;

    public int waterCount;
    public int mushroomCount;
     public TextMeshProUGUI fruitText;
    public TextMeshProUGUI mushroomText;
     public TextMeshProUGUI buyLandText;
     public TextMeshProUGUI waterText;
     public TextMeshProUGUI oreText;

      public string fruitType = "Wheat";

       public Button wheatButton;
    public Button fruitButton;


    public static GameManager Instance;

    public bool isWateringAnim = false;
    public bool isHarvestingAnim = false;

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

    void Start()
    {
        LoadGameData();
        
    }

    void Update()
    {
        UpdateUI();
    }
    public void CollectFruit()
    {
        fruitCount++;
        SaveGameData();

    }

    public void CollectWater()
    {
        waterCount++;
        SaveGameData();
    }

    void UpdateUI()
    {
        if (fruitText != null) fruitText.text = fruitCount.ToString();
        if (mushroomText != null) mushroomText.text = mushroomCount.ToString();
        if (waterText != null) waterText.text = waterCount.ToString();
        if (buyLandText != null) buyLandText.text = "Buy Land (" + landCost + " Ore)";
        if (oreText != null) oreText.text = oreCount.ToString();
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
        SaveGameData();
    }

    public bool WaterPlant()
    {
        if (waterCount > 0)
        {
            waterCount--;
            SaveGameData();
            return true;
            
        }
        return false;
        
    }

    public void BuyLand()
    {
        if (oreCount >= landCost)
        {   
            oreCount -= landCost;
            landCost = landCost * 10;
           
            ExpandLength();
            landSize++;
        }
        SaveGameData();
    }

    public void ExpandLength()
    {
        // Move the left fence 4 tiles to the left
        if (leftFenceTilemap != null)
        {
            Vector3 position = leftFenceTilemap.transform.position;
            leftFenceTilemap.transform.position = new Vector3(position.x - 4, position.y, position.z);
        }

        // Duplicate and position the top and bottom fence
        if (topFenceTilemap != null)
        {
            Vector3 topFencePosition = topFenceTilemap.transform.position;
            GameObject newTopFence = Instantiate(topFenceTilemapPrefab, new Vector3(topFencePosition.x - (landSize * 4), topFencePosition.y, topFencePosition.z), Quaternion.identity);
            newTopFence.transform.SetParent(Grid);
            newTopFence.name = "Top&BottomFence (Duplicate)";
        }

        // Duplicate and position the hill
        
            Vector3 hillPosition = topFenceTilemap.transform.position;
            GameObject newHill = Instantiate(hillTilemapPrefab, new Vector3(hillPosition.x - (landSize * 4), hillPosition.y, hillPosition.z), Quaternion.identity);
            newHill.transform.SetParent(Grid);
            newHill.name = "Hill (Duplicate)";

            foreach (GameObject boundary in boundaries)
            {
                Vector3 boundaryPosition = boundary.transform.position;
                GameObject newBoundary = Instantiate(boundary, new Vector3(boundaryPosition.x - (landSize * 4), boundaryPosition.y, boundaryPosition.z), Quaternion.identity);
            }
              Vector3 leftBoundaryPosition = leftBoundary.transform.position;
            leftBoundary.transform.position = new Vector3(leftBoundaryPosition.x - 4, leftBoundaryPosition.y, leftBoundaryPosition.z);


                
        


    }

    public void LeaderboardButton()
    {
        SaveGameData();
        SceneManager.LoadScene("Leaderboard");
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("OreCount", oreCount);
        PlayerPrefs.SetInt("FruitCount", fruitCount);
        PlayerPrefs.SetInt("WaterCount", waterCount);
        PlayerPrefs.SetInt("MushroomCount", mushroomCount);
        PlayerPrefs.SetInt("LandCost", landCost);
        PlayerPrefs.SetInt("LandSize", landSize);
        PlayerPrefs.SetString("FruitType", fruitType);
        PlayerPrefs.Save();
    }

    public void LoadGameData()
    {
        oreCount = PlayerPrefs.GetInt("OreCount", 1000);
        fruitCount = PlayerPrefs.GetInt("FruitCount", 0);
        waterCount = PlayerPrefs.GetInt("WaterCount", 0);
        mushroomCount = PlayerPrefs.GetInt("MushroomCount", 0);
        landCost = PlayerPrefs.GetInt("LandCost", 10);
        landSize = PlayerPrefs.GetInt("LandSize", 1);
        fruitType = PlayerPrefs.GetString("FruitType", "Wheat");
    }
    
}
