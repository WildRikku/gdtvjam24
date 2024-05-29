using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BattleField battleField;
    public KeyCode activationKey = KeyCode.A;

    public bool isActive;

    public event EventHandler AttackFinished;
    
    public virtual void Trigger()
    {
        Debug.Log("Default Weapon base class Boom");
    }

    protected void OnAttackFinished()
    {
        AttackFinished?.Invoke(this, EventArgs.Empty);
    }
}
