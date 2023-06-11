using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{

   [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget {get; private set;}

    private void Start(){
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other){
        if (!other.TryGetComponent<Target>(out Target target)){return ;}
        if(target == null ){return;}
        targets.Add(target);
        target.onDestroyed += RemoveTarget;
    }
    
    private void OnTriggerExit(Collider other){
       if (!other.TryGetComponent<Target>(out Target target)){return ;}
        RemoveTarget(target);
    }

    public bool SelectTarget(){
        if(targets.Count == 0){ return false;}

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets)
        {
            if(target == null){continue;}
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);


            Renderer targetRenderer = target.GetComponentInChildren<Renderer>();
            if(targetRenderer == null || !targetRenderer.isVisible){
                continue;
            }
            
            Vector2 toCenter = viewPos - new Vector2(0.5f,0.5f);

            if(toCenter.sqrMagnitude < closestTargetDistance){
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if(closestTarget == null){ return false; }

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel(){
        if(CurrentTarget == null){return;}
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    public void RemoveTarget(Target target){
        if (target.gameObject.GetComponent<AudioController>() != null){
            target.gameObject.GetComponent<AudioController>().SetIsMonsterAttacking(false);
        }

        if(CurrentTarget == target){
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.onDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public bool GetIsTargetsNear()
    {
        return targets.Count > 1;
    }

    public List<Target> GetPlayerTargets()
    {
        return targets;
    }

    public void RestoreTargets(List<Target> newTargets)
    {
        this.targets = newTargets;
    } 

}
