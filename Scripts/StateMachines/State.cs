using UnityEngine;

public abstract class State
{
 
    public abstract void Enter();

    public abstract void Tick(float deltaTime);
    
    public abstract void Exit();

    protected float GetNormalizeTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo =  animator.GetNextAnimatorStateInfo(0);
            
        if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
       
        }else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag)){
            return currentInfo.normalizedTime;
       
        }else{
            return 0f;
        }
    }
}
