using UnityEngine;
// Faire suivre la barre de vie à Olivia
public class HealthBarFollowScript : MonoBehaviour
{
    [SerializeField] Transform _oliviaTransform;
    void LateUpdate()
    {
        Vector3 position = new Vector3(
        _oliviaTransform.position.x,
        _oliviaTransform.position.y + 2f,
        _oliviaTransform.position.z);
        transform.position = position;
    }

}
