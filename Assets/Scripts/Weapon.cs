using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BattleField battleField;
    
    public virtual void Trigger()
    {
        Debug.Log("Boom");
    } 
}
