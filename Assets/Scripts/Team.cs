using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Team : MonoBehaviour
{
    public const short MaxTeamMembers = 5;
    
    public GameObject playerPrefab;
    private Dictionary<short, PlayerController> members;
    private short activeMember;
    
    public List<GameObject> weaponPrefabs;
    public event EventHandler turnEnded;
    [HideInInspector]
    public int teamColor;

    public Collider2D spawnZone;
    
    void Start()
    {
        members = new();
        
        Bounds bounds = spawnZone.bounds;
        for (short i = 0; i < MaxTeamMembers; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab, bounds.min + bounds.size/MaxTeamMembers * i, Quaternion.identity, transform);
            PlayerController pc = newPlayer.GetComponent<PlayerController>();
            SpiderChangeTeamColor spiderChangeTeamColor = newPlayer.GetComponentInChildren<SpiderChangeTeamColor>();
            spiderChangeTeamColor.teamColor = teamColor;
            pc.mainWeapon = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)];
            pc.index = i;
            pc.HealthUpdated += TeamMemberOnHealthUpdated;
            members.Add(i, pc);
        }

        activeMember = 0;
    }

    private void TeamMemberOnHealthUpdated(object sender, EventArgs e)
    {
        PlayerController pc = (PlayerController)sender;
        if (pc.Health <= 0)
        {
            // RIP
            members.Remove(pc.index);
        }
    }

    public void PlayerAction()
    {
        members.ElementAt(activeMember).Value.Attack();
        members.ElementAt(activeMember).Value.AttackFinished += OnAttackFinished;
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
