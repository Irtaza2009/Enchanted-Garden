using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
      
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void AnimateMovement()
    {
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetBool("isMoving", true);

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
            animator.SetBool("isMoving", false);
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
                else if (plot.WaterPlant())
                {
                    Debug.Log("Watering plant");
                }
                else
                {
                    plot.PlantSeed(); // Plant a seed
                    Debug.Log("Planting seed");
                }
            }
        }
    }



}
