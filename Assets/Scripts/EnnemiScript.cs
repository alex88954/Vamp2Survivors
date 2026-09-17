using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// Script principal de l'ennemi
public class EnnemiScript : MonoBehaviour, IDamageable
{
    private GameObject _olivia;
    private int hp = 10;
    private NavMeshAgent navmesh;
    private Animator animator;
    private bool _estPeureux;
    private bool _hasAttackAnimation;
    public EtatAttaque etatAttaque;
    public EtatPoursuite etatPoursuite;
    public EtatFuite etatFuite;
    public EtatEnnemi etatCourant;
    [SerializeField] private Light spotlight;
    private Coroutine attackCoroutine;
    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        etatAttaque = new EtatAttaque(this, navmesh);
        etatPoursuite = new EtatPoursuite(this, navmesh);
        etatFuite = new EtatFuite(this, navmesh);
        etatCourant = etatPoursuite;
    }
    void Update()
    {
        etatCourant.Execute();
    }
    // Changement d'état
    public void ChangeEtat(EtatEnnemi nouvelEtat)
    {
        if (etatCourant == nouvelEtat) return;
        etatCourant?.Exit();
        etatCourant = nouvelEtat;
        etatCourant.Enter();
    }
    // Attaquer Olivia
    public void Attack()
    {
        //Debug.Log("ENEMY ATTACK");
        _olivia.GetComponent<OliviaScript>().TakeDamage(2);
    }
    // Commencer la coroutine d'attaque
    public void StartAttack()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackLoop());
        }
    }
    // Coroutine d'attaque
    private IEnumerator AttackLoop()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(2);
        }
    }
    // Arrêter la coroutine d'attaque
    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    // Prendre des dégâts
    public void TakeDamage(int damage)
    {
        hp -= damage;
        StartCoroutine(FlashRed());
        if (hp <= 0)
        {
            Mourir();
        }
    }
    // Activer spotlight
    private IEnumerator FlashRed()
    {
        spotlight.enabled = true;
        yield return new WaitForSeconds(0.15f);
        spotlight.enabled = false;
    }
    // Gérer la mort de l'ennemi
    private void Mourir()
    {
        GameEvents.OnMonsterDied?.Invoke(gameObject);
        Destroy(gameObject);
    }
    // Initialiser olivia
    public void SetOlivia(GameObject olivia)
    {
        _olivia = olivia;
    }
    // Obtenir la position d'olivia
    public Vector3 GetOliviaPosition()
    {
        return _olivia.transform.position;
    }
    public int GetHp()
    {
        return hp;
    }
    // Obtenir les dégâts d'attaque d'olivia
    public int GetOliviaDamage()
    {
        return _olivia.GetComponent<OliviaScript>().GetAttackDamage(); ;
    }
    public void SetEstPeureux(bool estPeureux)
    {
        _estPeureux = estPeureux;
    }
    public bool EstPeureux()
    {
        return _estPeureux;
    }
    public void SetHasAttackAnimation(bool hasAttackAnimation)
    {
        _hasAttackAnimation = hasAttackAnimation;
    }

    // Gérer les animations d'attaque et de fuite
    public void SetAnimationAttaque()
    {
        if (!_hasAttackAnimation) return;
        animator.SetBool($"Attaque{Random.Range(1, 4)}", true);
    }
    public void UnSetAnimationAttaque()
    {
        if (!_hasAttackAnimation) return;
        for (int i = 1; i < 4; i++)
        {
            animator.SetBool($"Attaque{i}", false);
        }
    }
    public void SetAnimationFuite()
    {
        if (!_estPeureux) return;
        animator.SetBool("Fuite", true);
    }

}

