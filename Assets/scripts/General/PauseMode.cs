using UnityEngine;
using UnityEngine.UI;

public class PauseMode : MonoBehaviour
{
    [SerializeField] private GameObject resumeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        resumeButton.SetActive(false);
    }
}
