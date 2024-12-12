using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Animator animator;
    private Vector2 movement;

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    private bool isOnSpot = false;
    private string currentSpotTag = "";

    private string playerDirection = "Down";

    private GameManager gameManager;

    public string playerState = "None";

    [SerializeField]
    private Button[] buttons;

    void Start()
    {
        gameManager = GameManager.Instance;

        animator = GetComponent<Animator>();

        dialogBox.SetActive(false);

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
        
      
    }

    private void ManageState()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToPlantState();
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

    private void ToPlantState()
    {
        playerState = "Plant";
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
        } else if (clickedButton.name == "PlantButton")
        {
            ToPlantState();
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.isWateringAnim)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
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
        } else if (other.CompareTag("ElixirSpot"))
        {
            isOnSpot = true;
            currentSpotTag = "ElixirSpot";
            ShowDialog(currentSpotTag);
        } else if (other.CompareTag("PlotW"))
        {
            isOnSpot = true;
            currentSpotTag = "PlotW";
            ShowDialog(currentSpotTag);
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plot") || other.CompareTag("ElixirSpot") || other.CompareTag("PlotW"))
        {
            isOnSpot = false;
            currentSpotTag = "";
            dialogBox.SetActive(false);
        }
    }

    void ShowDialog(string spotTag)
    {
        dialogBox.SetActive(true);
        if (spotTag == "Plot")
        {
            dialogText.text = "Press Enter to plant or harvest";
        } else if (spotTag == "ElixirSpot")
        {
            dialogText.text = "Press Enter to collect the elixir";
        } else if (spotTag == "PlotW")
        {
            dialogText.text = "Press Enter to water the plant";
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
                if (plot.Harvest())
                {
                    gameManager.CollectFruit(); // Collect the fruit or vegetable
                }
                else if (playerState == "Watering")
                {
                    if (plot.WaterPlant())
                    {

                        gameManager.isWateringAnim = true;
                    }
                }
                else
                {
                    plot.PlantSeed(); // Plant a seed
                    Debug.Log("Planting seed");
                }
            } else if (hitCollider.CompareTag("ElixirSpot"))
            {
                gameManager.CollectWater();
                Debug.Log("Collecting water");
                Debug.Log(gameManager.waterCount);
            }
        }
    }



}
