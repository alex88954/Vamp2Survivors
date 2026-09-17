using UnityEngine.AI;

public class EtatAttaque : EtatEnnemi
{
    public EtatAttaque(EnnemiScript ennemiScript, NavMeshAgent navmesh) : base(ennemiScript, navmesh) { }
    public override void Enter()
    {
        navmesh.isStopped = true;
        ennemiScript.SetAnimationAttaque();
        ennemiScript.StartAttack();
    }
    public override void ExecuteState()
    {
        if (GetDistanceToOlivia() >= 7)
        {
            ennemiScript.ChangeEtat(ennemiScript.etatPoursuite);
        }
    }
    public override void Exit()
    {
        navmesh.isStopped = false;
        ennemiScript.StopAttack();
        ennemiScript.UnSetAnimationAttaque();
    }
}