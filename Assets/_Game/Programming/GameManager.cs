using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Important Components")]
    [SerializeField] private GameObject playerInput;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject clearScreen;
    [SerializeField] private GameObject resetScreen;

    // Variables Needed
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayVictory()
    {
        playerInput.SetActive(false);
        winScreen.SetActive(true);
    }
    public void DisplayClear(bool display)
    {
        playerInput.SetActive(!display);
        clearScreen.SetActive(display);
    }
    public void DisplayReset(bool display)
    {
        playerInput.SetActive(!display);
        resetScreen.SetActive(display);
    }
}