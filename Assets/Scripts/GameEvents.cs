using System;
using UnityEngine;
// Définir tous les évènements du jeu
public class GameEvents
{
    public static Action<int> OnGameOver;
    public static Action<int> OnBioPointsChanged;
    public static Action<int> OnMonstersAliveChanged;
    public static Action<int> OnMonsterKilled;
    public static Action<GameObject> OnMonsterDied;

    public static Action OnUpgrade;
    public static Action<int> OnHpUpgrade;
    public static Action<int> OnRangeUpgrade;
    public static Action<int> OnAttackLvlUpgrade;

    public static Action OnOliviaHurt;
}