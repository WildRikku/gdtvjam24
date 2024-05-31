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
    private int _activeMember;
    
    public List<GameObject> weaponPrefabs;
    public List<Weapon> weapons;
    public event EventHandler TurnEnded;
    public event TeamUpdated TeamUpdated;
    [HideInInspector]
    public int teamColor;

    public short index;

    public Collider2D spawnZone;

    private GameController gameController;
    
    void Start()
    {
        _members = new();
        gameController = GameObject.Find("GameManagement").GetComponent<GameController>();

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
            if (gameController.botNames.Count > 0)
            {
                int value = Random.Range(0, gameController.botNames.Count);
                pc.botName = gameController.botNames[value];
                gameController.botNames.RemoveAt(value);
            }
            _members.Add(i, pc);
        }

        foreach (GameObject go in weaponPrefabs)
        {
            weapons.Add(go.GetComponent<Weapon>());
        }

        _activeMember = 0;
    }

    private void TeamMemberOnHealthUpdated(PlayerController pc)
    {
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
        _members.ElementAt(_activeMember).Value.TurnFinished += OnTurnFinished;
    }

    public PlayerController GetActivePlayer()
    {
        if (_activeMember >= _members.Count) _activeMember = _members.Count - 1; // a team member died between turns
        return _members.ElementAt(_activeMember).Value;
    }

    private void OnTurnFinished(PlayerController pc)
    {
        GetActivePlayer().TurnFinished -= OnTurnFinished;
        _activeMember++;
        if (_activeMember == _members.Count)
            _activeMember = 0;
        TurnEnded?.Invoke(this, EventArgs.Empty);
    }
}
