using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Animator animator;
    private Vector2 movement;

    public Joystick joystick;

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    private bool isOnSpot = false;
    private string currentSpotTag = "";

    private string playerDirection = "Down";

    private GameManager gameManager;

    public string playerState = "Harvesting";

    // Adding references to the buttons
    [SerializeField]
    private GameObject plantHarvestButton;

    [SerializeField]
    private GameObject waterCollectButton;

    [SerializeField]
    private GameObject waterButton;

    private bool canPlantHarvest = false;
    private bool canWaterCollect = false;
    private bool canWater = false;

    [SerializeField]
    private Button[] buttons;

    void Start()
    {
        gameManager = GameManager.Instance;

        animator = GetComponent<Animator>();

        dialogBox.SetActive(false);

        // Hide buttons initially
        plantHarvestButton.SetActive(false);
        waterCollectButton.SetActive(false);

        if (Input.touchSupported)
        {
            Debug.Log("Touch supported");
            joystick.gameObject.SetActive(true);
            
        }
        else
        {
            Debug.Log("Touch not supported");
        }

    }

    void Update()
    {
        ProcessInputs();
        AnimateMovement();

         if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckPlotInteraction();
        }

        ManageState();

        if (isOnSpot && !string.IsNullOrEmpty(currentSpotTag))
        {
            ShowDialog(currentSpotTag); // Continuously update dialog text
        }

        UpdateButtonStates();
        
      
    }



    private void ManageState()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToHarvestingState();
            SetAllButtonsInteractable();
            buttons[0].interactable = false;

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToWateringState();
            SetAllButtonsInteractable();
            buttons[1].interactable = false;
        }
    }

    private void ToHarvestingState()
    {
        playerState = "Harvesting";
    }

    private void ToWateringState()
    {
        playerState = "Watering";
    }

    public void SetAllButtonsInteractable()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void StateButtonHandler(Button clickedButton)
    {
        Debug.Log(clickedButton.name);
        SetAllButtonsInteractable();

        clickedButton.interactable = false;

        if (clickedButton.name == "WateringCan")
        {
            ToWateringState();
        } else if (clickedButton.name == "Axe")
        {
            ToHarvestingState();
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.isWateringAnim || !gameManager.isHarvestingAnim)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        if (Input.touchSupported)
        {
            float horizontalJ = joystick.Horizontal;
            float verticalJ = joystick.Vertical;

            // Determine which axis to prioritize
            if (Mathf.Abs(horizontalJ) > Mathf.Abs(verticalJ))
            {
                movement.x = horizontalJ;
                movement.y = 0; // Disable vertical movement
            }
            else
            {
                movement.x = 0; // Disable horizontal movement
                movement.y = verticalJ;
            }
        }
        
        
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Determine which axis to prioritize
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                movement.x = horizontal;
                movement.y = 0; // Disable vertical movement
            }
            else
            {
                movement.x = 0; // Disable horizontal movement
                movement.y = vertical;
            }
        
    }

    void AnimateMovement()
    {
        if (gameManager.isWateringAnim)
        {
            if (playerDirection == "Right")
            {
                animator.Play("WateringRight");
            } else if (playerDirection == "Left")
            {
                animator.Play("WateringLeft");
            } else if (playerDirection == "Up")
            {
                animator.Play("WateringUp");
            } else if (playerDirection == "Down")
            {
                animator.Play("WateringDown");
            }
        } else if (gameManager.isHarvestingAnim)
        {
            if (playerDirection == "Right")
            {
                animator.Play("HarvestingRight");
            }
            else if (playerDirection == "Left")
            {
                animator.Play("HarvestingLeft");
            }
            else if (playerDirection == "Up")
            {
                animator.Play("HarvestingUp");
            }
            else if (playerDirection == "Down")
            {
                animator.Play("HarvestingDown");
            }
        }
        
        else if (movement != Vector2.zero)
        {
           

            if (movement.x > 0)
            {
                animator.Play("WorkerRight");
                playerDirection = "Right";
            }
            else if (movement.x < 0)
            {
                animator.Play("WorkerLeft");
                playerDirection = "Left";
            }
            else if (movement.y > 0)
            {
                animator.Play("WorkerUp");
                playerDirection = "Up";
            }
            else if (movement.y < 0)
            {
                animator.Play("WorkerDown");
                playerDirection = "Down";
            }
        }
        else
        {

            if (playerDirection == "Right")
            {
                animator.Play("WorkerIdleRight");
            }
            else if (playerDirection == "Left")
            {
                animator.Play("WorkerIdleLeft");
            }
            else if (playerDirection == "Up")
            {
                animator.Play("WorkerIdleUp");
            }
            else if (playerDirection == "Down")
            {
            animator.Play("WorkerIdle");
            }
        }
    }

    void Move()
    {
        Vector3 newPosition = transform.position + (Vector3)movement * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plot"))
        {
            isOnSpot = true;
            currentSpotTag =  other.CompareTag("Plot") ? "Plot" : "";
            ShowDialog(currentSpotTag);
            canPlantHarvest = true;
        } else if (other.CompareTag("ElixirSpot"))
        {
            isOnSpot = true;
            currentSpotTag = "ElixirSpot";
            ShowDialog(currentSpotTag);
            canWaterCollect = true;
        } else if (other.CompareTag("PlotW"))
        {
            isOnSpot = true;
            currentSpotTag = "PlotW";
            ShowDialog(currentSpotTag);
            canWater = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plot") || other.CompareTag("ElixirSpot") || other.CompareTag("PlotW"))
        {
            isOnSpot = false;
            currentSpotTag = "";
            dialogBox.SetActive(false);
            // Reseting interactivity when leaving the plot
            canPlantHarvest = false;
            canWaterCollect = false;
        }
    }

    void ShowDialog(string spotTag)
    {
        dialogBox.SetActive(true);

        bool isTouchDevice = Input.touchSupported;

        if (spotTag == "Plot")
        {
            if (playerState == "Harvesting")
            {
                dialogText.text = isTouchDevice 
                    ? "Tap the button to plant or harvest" 
                    : "Press Enter to plant or harvest";
            }
            else
            {
                dialogText.text = isTouchDevice 
                    ? "Switch to harvesting/planting by tapping the button" 
                    : "Switch to harvesting/planting by pressing 1";
            }
        }
        else if (spotTag == "PlotW")
        {
            if (playerState == "Watering")
            {
                dialogText.text = isTouchDevice 
                    ? "Tap the button to water the plant" 
                    : "Press Enter to water the plant";
            }
            else
            {
                dialogText.text = isTouchDevice 
                    ? "Switch to watering by tapping the button" 
                    : "Switch to watering by pressing 2";
            }
        }
        else if (spotTag == "ElixirSpot")
        {
            dialogText.text = isTouchDevice 
                ? "Tap the button to collect the elixir" 
                : "Press Enter to collect the elixir";
        }
    }



    void CheckPlotInteraction()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var hitCollider in hitColliders)
        {
            Plot plot = hitCollider.GetComponent<Plot>();
            if (plot != null)
            {
                if (playerState == "Harvesting")
                {
                    Debug.Log("Harvesting");
                    if (plot.Harvest())
                    {
                        gameManager.isHarvestingAnim = true;
                        gameManager.CollectFruit(); // Collect the fruit or vegetable
                    } 
                    else
                    {
                        plot.PlantSeed(); // Plant a seed
                        Debug.Log("Planting seed");
                    }
                }
                else if (playerState == "Watering")
                {
                    if (plot.WaterPlant())
                    {

                        gameManager.isWateringAnim = true;
                    }
                }
               
            } else if (hitCollider.CompareTag("ElixirSpot"))
            {
                gameManager.CollectWater();
                Debug.Log("Collecting water");
                Debug.Log(gameManager.waterCount);
            }
        }
    }

    public void OnPlantHarvestButtonClick()
    {
        if (canPlantHarvest)
        {
            CheckPlotInteraction();
            // Plant or harvest logic
            Debug.Log("Planting/Harvesting crops");

        }
    }

    public void OnWaterCollectButtonClick()
    {
        if (canWaterCollect)
        {
            CheckPlotInteraction();
            // Water or collect elixir logic
            Debug.Log("Collecting elixir");
        }
    }

    public void OnWateringButtonClick()
    {
        if (canWater)
        {
            CheckPlotInteraction();
            // Water or collect elixir logic
            Debug.Log("Watering plants");
        }
    }

        // Method to update button visibility and interactivity
    void UpdateButtonStates()
    {
        if (canPlantHarvest && playerState == "Harvesting")
        {
            plantHarvestButton.SetActive(true);
            plantHarvestButton.GetComponent<Button>().interactable = true;
        }
        else 
        {
            plantHarvestButton.GetComponent<Button>().interactable = false;
        }

        if (canWaterCollect )
        {
            waterCollectButton.SetActive(true);
            waterCollectButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            waterCollectButton.GetComponent<Button>().interactable = false;
        }

        if (canWater && playerState == "Watering")
        {
            waterButton.SetActive(true);
            waterButton.GetComponent<Button>().interactable = true;
        }
        else 
        {
            waterButton.GetComponent<Button>().interactable = false;
        }

        // Hide buttons if the player is not near any plot
        if (!canPlantHarvest && !canWaterCollect && !canWater)
        {
            plantHarvestButton.SetActive(false);
            waterCollectButton.SetActive(false);
            waterButton.SetActive(false);
        }
    }




}
