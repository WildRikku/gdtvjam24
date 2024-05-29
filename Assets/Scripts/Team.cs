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
    [HideInInspector]
    public int teamColor;

    public Collider2D spawnZone;
    
    void Start()
    {
        members = new();
        short maxTeamMembers = 5;
        Bounds bounds = spawnZone.bounds;
        for (short i = 0; i < maxTeamMembers; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab, bounds.min + bounds.size/maxTeamMembers * i, Quaternion.identity, transform);
            PlayerController pc = newPlayer.GetComponent<PlayerController>();
            SpiderChangeTeamColor spiderChangeTeamColor = newPlayer.GetComponentInChildren<SpiderChangeTeamColor>();
            spiderChangeTeamColor.teamColor = teamColor;
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
