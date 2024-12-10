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
            }
            else if (movement.x < 0)
            {
                animator.Play("WorkerLeft");
            }
            else if (movement.y > 0)
            {
                animator.Play("WorkerUp");
            }
            else if (movement.y < 0)
            {
                animator.Play("WorkerDown");
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            animator.Play("WorkerIdle");
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
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plot"))
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
                else
                {
                    plot.PlantSeed(); // Plant a seed
                }
            }
        }
    }



}
