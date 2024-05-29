using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BattleField battleField;
    public KeyCode activationKey = KeyCode.A;
    
    
    
    public virtual void Trigger()
    {
        Debug.Log("Default Weapon base class Boom");
    } 
}
