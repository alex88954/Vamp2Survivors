using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
// Contrôler les scènes du jeu
public class SceneControllerScript : MonoBehaviour
{
    // Quitter le jeu
    public void Quitter()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    // Démarrer le jeu avec la difficulté choisie
    public void StartGame(int difficulty)
    {
        if (difficulty == 0)
        {
            DifficultyManager.instance.SetDifficulty(14, 30, 5);
        }
        else
        {
            DifficultyManager.instance.SetDifficulty(8, 50, 10);
        }
        SceneManager.LoadSceneAsync(1);
    }
    // Changer de scène
    public void ChangeScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }
}
