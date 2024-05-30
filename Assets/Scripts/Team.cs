using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void TeamUpdated(short teamIndex, int memberCount);

public class Team : MonoBehaviour
{
    public const short MaxTeamMembers = 5;
    
    public GameObject playerPrefab;
    private Dictionary<short, PlayerController> _members;
    private short _activeMember;
    
    public List<GameObject> weaponPrefabs;
    public event EventHandler TurnEnded;
    public event TeamUpdated TeamUpdated;
    [HideInInspector]
    public int teamColor;

    public short index;

    public Collider2D spawnZone;
    
    void Start()
    {
        _members = new();
        
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
            _members.Add(i, pc);
        }

        _activeMember = 0;
    }

    private void TeamMemberOnHealthUpdated(object sender, EventArgs e)
    {
        PlayerController pc = (PlayerController)sender;
        if (pc.Health <= 0)
        {
            // RIP
            _members.Remove(pc.index);
            TeamUpdated?.Invoke(index, _members.Count);
        }
    }

    public void PlayerAction()
    {
        _members.ElementAt(_activeMember).Value.Attack();
        _members.ElementAt(_activeMember).Value.AttackFinished += OnAttackFinished;
    }

    public PlayerController GetActivePlayer()
    {
        return _members.ElementAt(_activeMember).Value;
    }

    private void OnAttackFinished(object sender, EventArgs e)
    {
        GetActivePlayer().AttackFinished -= OnAttackFinished;
        _activeMember++;
        if (_activeMember == _members.Count)
            _activeMember = 0;
        TurnEnded?.Invoke(this, EventArgs.Empty);
    }
}
