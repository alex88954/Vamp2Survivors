using System;
using UnityEngine;
// Définir les évènements pour les cheat inputs
public class CheatEvents
{
    public static Action OnReplinishHp;
    public static Action OnSpawnBouleBio;
    public static Action<int> OnGiveBioPoints;
    public static Action<int> OnRemoveHp;
    public static Action OnClearEnemies;
}