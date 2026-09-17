using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// Gérer les backgrounds 
public class BackgroundManagerScript : MonoBehaviour
{
    [SerializeField] private Image[] backgrounds;
    private int current = 0;

    void Start()
    {
        foreach (Image image in backgrounds)
        {
            image.enabled = false;
        }
        StartCoroutine(BackgroundLoop());
    }
    // Boucle pour changer les backgrounds à des intervalles aléatoires
    private IEnumerator BackgroundLoop()
    {
        while (true)
        {

            float delay = Random.Range(0, 0.5f);

            ActivateNew();
            DeactivateOld();

            yield return new WaitForSeconds(delay);
        }
    }
    // Activer un background aléatoire
    private void ActivateNew()
    {
        current = Random.Range(0, backgrounds.Length);
        backgrounds[current].enabled = true;
    }
    // Désactiver l'ancien background
    private void DeactivateOld()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (i != current)
            {
                backgrounds[i].enabled = false;
            }
        }
    }
}
