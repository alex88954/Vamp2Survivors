using UnityEngine;
using UnityEngine.AI;

public class EtatFuite : EtatEnnemi
{
    public EtatFuite(EnnemiScript ennemiScript, NavMeshAgent navmesh) : base(ennemiScript, navmesh) { }
    public override void Enter()
    {
        ennemiScript.SetAnimationFuite();
        navmesh.speed *= 2;
    }
    public override void ExecuteState()
    {
        Vector3 directionFuite = (ennemiScript.transform.position - ennemiScript.GetOliviaPosition()).normalized;
        Vector3 destinationFuite = ennemiScript.transform.position + directionFuite * 10f;
        navmesh.SetDestination(destinationFuite);
    }





}