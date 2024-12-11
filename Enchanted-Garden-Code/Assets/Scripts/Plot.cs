using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Plot : MonoBehaviour
{
    private Animator animator;
    
    [SerializeField]
    private bool isPlanted = false;
    private bool isHarvestable = false;
    private bool isWaterable = false;

    private GameManager gameManager;

    

    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
    }

    public void PlantSeed()
    {
        Debug.Log(isPlanted);
        if (!isPlanted)
        {
            Debug.Log("Planting seed in plot");
            StartCoroutine(GrowPlant());
        }
    }

    public bool WaterPlant()
    {
        if (isWaterable)
        {
            StartCoroutine(GrowPlant2());
            return true;
        }
        return false;
    }


    private IEnumerator GrowPlant()
    {
        isPlanted = true;
        Debug.Log(GameManager.Instance);

        if (gameManager.fruitType == "Wheat")
        {
            animator.Play("WheatGrowth1");
        }
        else
        {
            animator.Play("FruitGrowth1");
        }
       
        yield return new WaitForSeconds(8f); // Wait for the animation to complete

        isWaterable = true;
        gameObject.tag = "PlotW";
    }

 private IEnumerator GrowPlant2()
    {


        if (gameManager.fruitType == "Wheat")
        {
            animator.Play("WheatGrowth2");
        }
        else
        {
            animator.Play("FruitGrowth2");
        }
       
        yield return new WaitForSeconds(14f); // Wait for the animation to complete
        isHarvestable = true;
        isWaterable = false;
        gameObject.tag = "Plot";
    }


    public bool Harvest()
    {
        if (isHarvestable)
        {
         //   FindObjectOfType<AudioManager>().Play("Harvest");
            animator.Play("PlotIdle"); // Optional: play a harvest animation or reset to idle
            isPlanted = false;
            isHarvestable = false;
            return true;
        }
        return false;
    }

    
}
