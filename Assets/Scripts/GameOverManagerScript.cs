using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Gérer l'écran de fin de partie
public class GameOverManagerScript : MonoBehaviour
{
    [SerializeField] private Canvas endScreenCanvas;
    [SerializeField] private RectTransform backgroundTrans;
    [SerializeField] private TMP_Text killsText;

    private float glitchIntensity = 300;
    private Coroutine glitchCoroutine;
    [SerializeField] private SceneControllerScript sceneController;


    // S'abonner au commencement de la partie
    void Start()
    {
        endScreenCanvas.gameObject.SetActive(false);
        GameEvents.OnGameOver += HandleGameOver;

    }
    // Se désabonner à la fin de la partie
    void OnDestroy()
    {
        GameEvents.OnGameOver -= HandleGameOver;
    }
    // Afficher l'image game over
    private void HandleGameOver(int kills)
    {
        SetKillsText(kills);
        endScreenCanvas.gameObject.SetActive(true);
        StartGlitch();
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    // Lancer le glitch
    public void StartGlitch()
    {
        if (glitchCoroutine == null)
        {
            glitchCoroutine = StartCoroutine(Glitch());
        }
    }
    // Coroutine pour faire le glitch
    private IEnumerator Glitch()
    {
        while (true)
        {
            float randomX = Random.Range(-glitchIntensity, glitchIntensity);

            backgroundTrans.anchoredPosition = new Vector2(randomX, 0);

            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    // Arrêter le glitch
    private void StopGlitch()
    {
        if (glitchCoroutine != null)
        {
            StopCoroutine(glitchCoroutine);
            glitchCoroutine = null;
            backgroundTrans.anchoredPosition = Vector2.zero;
        }
    }
    // Retourner au menu après un délai
    IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSecondsRealtime(4);
        StopGlitch();
        sceneController.ChangeScene(0);
    }
    // Mettre à jour le texte des kills
    private void SetKillsText(int kills)
    {
        killsText.text = $"<color=green>{kills}</color>     Creatures contenues";
    }

}
