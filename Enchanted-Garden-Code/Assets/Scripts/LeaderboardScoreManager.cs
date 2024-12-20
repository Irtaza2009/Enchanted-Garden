using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LeaderboardScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text, GameManager.Instance.fruitCount + GameManager.Instance.mushroomCount + GameManager.Instance.oreCount);
    }

}
