using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FarmAnimalBaseState : State
{

    protected FarmAnimalStateMachine stateMachine;

    public FarmAnimalBaseState(FarmAnimalStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
