using UnityEngine;
using UnityEngine.UI;
// Gérer le canvas d'upgrade et les boutons d'upgrade
public class UpgradeManagerScript : MonoBehaviour
{
    [SerializeField] private Canvas upgradeCanvas;


    void Start()
    {
        GameEvents.OnUpgrade += OpenUpgradePanel;
    }
    void OnDestroy()
    {
        GameEvents.OnUpgrade -= OpenUpgradePanel;
    }
    // Ouvrir le canvas d'upgrade et mettre le jeu en pause
    private void OpenUpgradePanel()
    {
        Time.timeScale = 0;
        upgradeCanvas.gameObject.SetActive(true);
    }
    // Fermer le canvas d'upgrade et reprendre le jeu
    private void CloseUpgradePanel()
    {
        upgradeCanvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    // Appeler les événements d'upgrade correspondants
    public void HpUpgrade(int addedHp)
    {
        GameEvents.OnHpUpgrade?.Invoke(addedHp);
        CloseUpgradePanel();
    }
    public void RangeUpgrade(int addedRange)
    {
        GameEvents.OnRangeUpgrade?.Invoke(addedRange);
        CloseUpgradePanel();
    }
    public void AttackLevelUpgrade(int addedAttLvl)
    {
        GameEvents.OnAttackLvlUpgrade?.Invoke(addedAttLvl);
        CloseUpgradePanel();
    }
}
