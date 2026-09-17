using UnityEngine;
using UnityEngine.AI;
public abstract class EtatEnnemi
{
    protected EnnemiScript ennemiScript;
    protected NavMeshAgent navmesh;
    public EtatEnnemi(EnnemiScript ennemiScript, NavMeshAgent navmesh)
    {
        this.ennemiScript = ennemiScript;
        this.navmesh = navmesh;
    }
    public virtual void Enter() { }
    public void Execute()
    {
        HasToEscape();
        ExecuteState();
    }
    public abstract void ExecuteState();
    public virtual void Exit() { }
    private void HasToEscape()
    {
        if (!ennemiScript.EstPeureux()) return;
        if (ennemiScript.GetHp() <= ennemiScript.GetOliviaDamage())
        {
            ennemiScript.ChangeEtat(ennemiScript.etatFuite);
        }
    }
    protected float GetDistanceToOlivia()
    {
        return Vector3.Distance(
            ennemiScript.transform.position,
            ennemiScript.GetOliviaPosition()
        );
    }
}