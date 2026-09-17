using TMPro;
using UnityEngine;

// Afficher les informations du jeu
public class GameInfoManagerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text bioPointsText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text monstersAliveText;

    void Start()
    {
        GameEvents.OnBioPointsChanged += UpdateBioPointsText;
        GameEvents.OnMonsterKilled += UpdateMonstersKilledText;
        GameEvents.OnMonstersAliveChanged += UpdateMonstersAliveText;

    }
    void Destroy()
    {
        GameEvents.OnBioPointsChanged -= UpdateBioPointsText;
        GameEvents.OnMonsterKilled -= UpdateMonstersKilledText;
        GameEvents.OnMonstersAliveChanged -= UpdateMonstersAliveText;
    }
    // Mettre à jour le texte points de bioresistance
    private void UpdateBioPointsText(int bioPoints)
    {
        bioPointsText.text = $"Bioresistance: {bioPoints}";
    }
    // Mettre à jour le texte nombre de monstres contenus
    private void UpdateMonstersKilledText(int monstersKilled)
    {
        killsText.text = $"Creatures contenues: {monstersKilled}";
    }
    // Mettre à jour le texte nombre monstres libres
    private void UpdateMonstersAliveText(int monstersAlive)
    {
        monstersAliveText.text = $"Creatures libres:            {monstersAlive}";
    }
}
