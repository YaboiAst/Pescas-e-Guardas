using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private string tutorialSceneName = "Tutorial - Rafa";

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == tutorialSceneName)
        {
            GameManager.Instance.SetTutorialMode(true);
            Debug.Log($"Tutorial iniciado na cena {tutorialSceneName}");
        }
        else
        {
            GameManager.Instance.SetTutorialMode(false);
            Debug.Log($"N�o est� no modo tutorial");
        }
    }
}
