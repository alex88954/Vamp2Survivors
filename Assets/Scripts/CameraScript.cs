using System.Collections;
using UnityEngine;
// Faire la caméra suivre Olivia
public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform _oliviaTransform;
    [SerializeField] float xOff;
    [SerializeField] float yOff;
    [SerializeField] float zOff;
    private bool isShaking = false;
    private Vector3 shakeOff;

    private void Start()
    {
        GameEvents.OnOliviaHurt += HandleOliviaHurt;
    }
    // Suivre Olivia et contrôler le l'activation du shake
    void LateUpdate()
    {
        Vector3 position = new Vector3(
        _oliviaTransform.position.x + xOff,
        _oliviaTransform.position.y + yOff,
        _oliviaTransform.position.z + zOff);
        transform.position = position + shakeOff;
    }
    // Activer le shake quand Olivia est touchée
    private void HandleOliviaHurt()
    {
        if (!isShaking)
        {
            StartCoroutine(CameraShake());
        }
    }
    // Faire trembler la caméra
    private IEnumerator CameraShake()
    {
        float timePassed = 0;
        while (timePassed < 0.15f)
        {
            isShaking = true;
            float x = Random.Range(-0.15f, 0.15f);
            float y = Random.Range(-0.15f, 0.15f);
            float z = Random.Range(-0.15f, 0.15f);
            shakeOff = new Vector3(x, y, z);
            timePassed += Time.deltaTime;
            yield return null;
        }
        shakeOff = Vector3.zero;
        isShaking = false;

    }
}
