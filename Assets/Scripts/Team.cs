using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Team : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<PlayerController> members;
    private int activeMember;
    public List<GameObject> weaponPrefabs;
    public event EventHandler turnEnded;
    public int teamColor;
    
    void Start()
    {
        members = new();
        
        for (short i = 0; i < 5; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab, transform);
            PlayerController pc = newPlayer.GetComponent<PlayerController>();
            pc.mainWeapon = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)]; 
            members.Add(pc);
        }

        activeMember = 0;
    }

    public void PlayerAction()
    {
        members[activeMember].Attack();
        members[activeMember].AttackFinished += OnAttackFinished;
    }

    public PlayerController GetActivePlayer()
    {
        return members[activeMember];
    }

    private void OnAttackFinished(object sender, EventArgs e)
    {
        members[activeMember].AttackFinished -= OnAttackFinished;
        activeMember++;
        if (activeMember == members.Count)
            activeMember = 0;
        turnEnded?.Invoke(this, EventArgs.Empty);
    }
}
