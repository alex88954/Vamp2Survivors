using UnityEngine;
using UnityEngine.InputSystem;

public class CheatInputManagerScript : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            CheatEvents.OnReplinishHp?.Invoke();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            CheatEvents.OnSpawnBouleBio?.Invoke();
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            CheatEvents.OnGiveBioPoints?.Invoke(5);
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            CheatEvents.OnGiveBioPoints?.Invoke(100);
        }
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            CheatEvents.OnRemoveHp?.Invoke(1);
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            CheatEvents.OnRemoveHp?.Invoke(999);
        }
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            CheatEvents.OnClearEnemies?.Invoke();
        }
    }
}
