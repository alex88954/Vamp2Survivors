using UnityEngine;
using UnityEngine.AI;

public class EtatPoursuite : EtatEnnemi
{
    public EtatPoursuite(EnnemiScript ennemiScript, NavMeshAgent navmesh) : base(ennemiScript, navmesh) { }
    public override void ExecuteState()
    {
        navmesh.SetDestination(ennemiScript.GetOliviaPosition());
        if (GetDistanceToOlivia() <= 5)
        {
            ennemiScript.ChangeEtat(ennemiScript.etatAttaque);
        }
    }
}