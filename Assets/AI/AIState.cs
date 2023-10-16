using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract void EnterState(AIController controller);
    public abstract void UpdateState(AIController controller);
    public abstract void ExitState(AIController controller);
    public abstract Vector2 GetDirection();
}
