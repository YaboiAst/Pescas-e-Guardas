using UnityEngine;

public class OpenCanvasOnEnter : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject canvasUI; // Referência ao Canvas que será aberto/fechado
    [SerializeField] private bool pauseGameWhenOpen = true; // Se true, pausa o jogo quando o Canvas está aberto

    private void Update()
    {
        // Verifica se o Enter (principal ou numérico) foi pressionado
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ToggleCanvas();
        }
    }

    private void ToggleCanvas()
    {
        if (canvasUI != null)
        {
            bool isCanvasActive = !canvasUI.activeSelf;
            canvasUI.SetActive(isCanvasActive);

            // Pausa o jogo se necessário
            if (pauseGameWhenOpen)
            {
                Time.timeScale = isCanvasActive ? 0f : 1f;
            }
        }
    }

    // Garante que o jogo não fique pausado se o objeto for desativado
    private void OnDisable()
    {
        if (pauseGameWhenOpen)
        {
            Time.timeScale = 1f;
        }
    }
}