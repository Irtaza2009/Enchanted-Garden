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

    void Start()
    {

        animator = GetComponent<Animator>();

        dialogBox.SetActive(false);

    }

    void Update()
    {
        ProcessInputs();
        AnimateMovement();

      
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
        else if (spotTag == "Plot")
        {
            dialogText.text = "Press Enter to plant or harvest";
        }
    }





}
