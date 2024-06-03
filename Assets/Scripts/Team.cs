using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void TeamUpdated(short teamIndex, int memberCount);

public class ActiveTeamMemberDeadException : Exception {
    public ActiveTeamMemberDeadException() {
    }

    public ActiveTeamMemberDeadException(string message)
        : base(message) {
    }

    public ActiveTeamMemberDeadException(string message, Exception inner)
        : base(message, inner) {
    }
}

public class Team : MonoBehaviour {
    public const short MaxTeamMembers = 4;

    public GameObject playerPrefab;
    private Dictionary<int, PlayerController> _members;
    private int _activeMember;

    public List<GameObject> weaponPrefabs;
    [HideInInspector]
    public List<Weapon> weapons;

    [HideInInspector]
    public int teamColor;
    public short index;
    public Collider2D spawnZone;

    private GameController _gameController;

    public event EventHandler TurnEnded;
    public event TeamUpdated TeamUpdated;

    private void Start() {
        _members = new();
        _gameController = GameObject.Find("GameManagement").GetComponent<GameController>();

        Bounds bounds = spawnZone.bounds;
        
        for (int k = 0; k < weaponPrefabs.Count; k++) {
            Weapon w = weaponPrefabs[k].GetComponent<Weapon>();
            w.index = k;
            weapons.Add(w);
        }
        
        for (short i = 0; i < MaxTeamMembers; i++) {
            GameObject newPlayer = Instantiate(playerPrefab, bounds.min + bounds.size / MaxTeamMembers * i,
                Quaternion.identity, transform);
            PlayerController pc = newPlayer.GetComponent<PlayerController>();
            SpiderChangeTeamColor spiderChangeTeamColor = newPlayer.GetComponentInChildren<SpiderChangeTeamColor>();
            spiderChangeTeamColor.teamColor = teamColor;
            pc.teamColor = teamColor;
            int randomWeaponIndex = Random.Range(0, weaponPrefabs.Count);
            pc.ChangeWeapon(weaponPrefabs[randomWeaponIndex], randomWeaponIndex);
            pc.index = i;
            pc.HealthUpdated += OnTeamMemberHealthUpdated;
            if (_gameController.botNames.Count > 0) {
                int value = Random.Range(0, _gameController.botNames.Count);
                pc.botName = _gameController.botNames[value];
                _gameController.botNames.RemoveAt(value);
            }

            pc.TurnFinished += OnPlayerTurnFinished;
            _members.Add(i, pc);
        }

        _activeMember = 0;
    }

    private void OnTeamMemberHealthUpdated(PlayerController pc) {
        if (pc.Health <= 0) {
            // RIP
            _members.Remove(pc.index);
            TeamUpdated?.Invoke(index, _members.Count);
        }
    }

    public void PlayerAction() {
        NextPlayer();
        GetActivePlayer().StartTurn();
    }

    /// <summary>
    /// Get the active player unit, if it is still alive.
    /// </summary>
    /// <param name="checkAlive">Decide if an exception should be raised if the active player is dead.</param>
    /// <returns></returns>
    /// <exception cref="ActiveTeamMemberDeadException"></exception>
    public PlayerController GetActivePlayer(bool checkAlive = true) {
        if (checkAlive && !_members.ContainsKey(_activeMember)) {
            throw new ActiveTeamMemberDeadException();
        }

        return !_members.ContainsKey(_activeMember) ? null : _members[_activeMember];
    }

    private void OnPlayerTurnFinished(PlayerController pc) {
        EndTurn();
        TurnEnded?.Invoke(this, EventArgs.Empty);
    }

    public void EndTurn(bool force = false) {
        if (force) {
            GetActivePlayer(false)?.EndTurn(true);
        }
    }

    public void NextPlayer() {
        do {
            _activeMember++;
            if (_activeMember == MaxTeamMembers) {
                _activeMember = 0;
            }
        } while (!_members.ContainsKey(_activeMember));
    }
}