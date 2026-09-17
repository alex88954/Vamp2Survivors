using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;
// Gérer Olivia
public class OliviaScript : MonoBehaviour, IDamageable
{
    CharacterController _cc;
    Animator _anim;
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float sprintMultiplier = 2f;
    private float moveSpeed;
    [SerializeField] float turnSpeed = 550f;
    private float gravity = -5f;
    private float directionY;
    private int hp;
    private int maxHp;
    [SerializeField] private Slider hpBar;
    private int niveauAttaque = 1;
    private int attackDamage = 5;
    public HashSet<IDamageable> ennemis = new HashSet<IDamageable>();
    public event Action<int> OnBioBallConsumed;
    public event Action OnOliviaDied;
    private SphereCollider _sphereCollider;
    [SerializeField] private VisualEffect _vfx;
    private int vfxRayonAttaque = 1;
    [SerializeField] private Light spotlight;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        sprint = InputSystem.actions.FindAction("Sprint");
        _sphereCollider = GetComponent<SphereCollider>();
        GameEvents.OnHpUpgrade += HandleHpUpgrade;
        GameEvents.OnRangeUpgrade += HandleRangeUpgrade;
        GameEvents.OnAttackLvlUpgrade += HandleAttackLVlUpgrade;

        CheatEvents.OnReplinishHp += ReplenishHp;
        CheatEvents.OnRemoveHp += TakeDamage;


        SetStats();
        StartCoroutine(AttackLoop());
    }
    void OnDestroy()
    {
        GameEvents.OnHpUpgrade -= HandleHpUpgrade;
        GameEvents.OnRangeUpgrade -= HandleRangeUpgrade;
        GameEvents.OnAttackLvlUpgrade -= HandleAttackLVlUpgrade;

        CheatEvents.OnReplinishHp -= ReplenishHp;
        CheatEvents.OnRemoveHp -= TakeDamage;
    }

    void Update()
    {
        // Gérer les déplacements d'Olivia
        Vector2 moveV = move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(moveV.x, 0, moveV.y);
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
        moveSpeed = !sprint.IsPressed() ? walkSpeed : walkSpeed * sprintMultiplier;
        // Gérer la gravité
        if (_cc.isGrounded)
        {
            directionY = -2f;
        }
        else
        {
            directionY += gravity * Time.deltaTime;
        }
        // Appliquer le mouvement
        Vector3 finalMove = direction * moveSpeed;
        finalMove.y = directionY;
        _cc.Move(finalMove * Time.deltaTime);
        _anim.SetFloat("Vitesse", direction.magnitude);
    }
    public void OnTriggerEnter(Collider other)
    {
        // Gérer la consommation de la boule de bioresistance
        if (other.gameObject.CompareTag("BouleBioresistance"))
        {
            OnBioBallConsumed?.Invoke(5);
            Destroy(other.gameObject);
        }
        // Gérer les ennemis dans le rayon d'attaque
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        IDamageable ennemi = other.GetComponent<IDamageable>();
        if (ennemi != null && ennemi is not OliviaScript)
            ennemis.Add(ennemi);
    }
    public void OnTriggerExit(Collider other)
    {
        // Gérer les ennemis qui sortent du rayon d'attaque
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        IDamageable ennemi = other.GetComponent<IDamageable>();
        if (ennemi != null && ennemi is not OliviaScript)
            ennemis.Remove(ennemi);
    }
    // Initialiser les stats d'Olivia en fonction de la difficulté
    private void SetStats()
    {
        if (DifficultyManager.instance.GetOliviaHp() == 0)
        {
            hp = 10;
            maxHp = 10;
        }
        else
        {
            hp = DifficultyManager.instance.GetOliviaHp();
            maxHp = hp;
        }
    }
    // Gérer l'attaque d'Olivia
    public void Attack()
    {
        int rng = UnityEngine.Random.Range(0, ennemis.Count);
        IDamageable ennemi = ennemis.ElementAt(rng);
        ennemi.TakeDamage(attackDamage);
        if ((ennemi as EnnemiScript).GetHp() <= 0)
        {
            ennemis.Remove(ennemi);
        }
    }
    private IEnumerator AttackLoop()
    {
        while (true)
        {
            if (ennemis.Count > 0)
            {
                Attack();
            }
            float tempsEntreAttaques = 1f / Mathf.Sqrt(niveauAttaque);
            yield return new WaitForSeconds(tempsEntreAttaques);
        }
    }
    // Gérer les dégâts subis par Olivia
    public void TakeDamage(int damage)
    {
        hp -= damage;
        GameEvents.OnOliviaHurt?.Invoke();
        StartCoroutine(FlashRed());
        SetHpBar();
        if (hp <= 0)
        {
            OnOliviaDied?.Invoke();
            Destroy(gameObject, 0.5f);
        }
    }
    // Activer le spotlight rouge pour indiquer qu'Olivia a été touchée
    private IEnumerator FlashRed()
    {
        spotlight.enabled = true;
        yield return new WaitForSeconds(0.15f);
        spotlight.enabled = false;
    }
    // Mettre à jour la barre de vie d'Olivia
    private void SetHpBar()
    {
        hpBar.value = (float)hp / maxHp;
    }
    public int GetAttackDamage()
    {
        return attackDamage;
    }
    public int GetHp()
    {
        return hp;
    }
    // Réinitialiser la vie d'Olivia à son maximum
    public void ReplenishHp()
    {
        hp = maxHp;
        SetHpBar();
    }
    // Gérer les upgrades d'Olivia
    private void HandleHpUpgrade(int hpAdded)
    {
        maxHp += hpAdded;
        ReplenishHp();
    }
    private void HandleRangeUpgrade(int rangeAdded)
    {
        _sphereCollider.radius += rangeAdded;
        vfxRayonAttaque += rangeAdded;
        _vfx.SetFloat("RayonAttaque", vfxRayonAttaque / 2);
    }
    private void HandleAttackLVlUpgrade(int attLvlAdded)
    {
        niveauAttaque += attLvlAdded;
    }
}
