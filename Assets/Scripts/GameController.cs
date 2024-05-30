using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [FormerlySerializedAs("Teams")] public List<Team> teams;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public GameMenuController menuController;

    public event EventHandler MatchStarted;

    private int _activeTeam;
    private bool _trackingProjectile;

    public List<String> botNames;  // use in PlayerController

    private void Awake()
    {
        Invoke(nameof(MatchBegin), 2f);
        bool[] usedTeamColors = new bool[4];
        
        for (short i = 0; i < teams.Count; i++)
        {
            int teamColor;
            do
            {
                teamColor = Random.Range(0, 4);
            } while (usedTeamColors[teamColor]);

            teams[i].teamColor = teamColor;
            if (menuController != null)
            {
                menuController.teamColorIndex[i] = teamColor;
            }
            usedTeamColors[teamColor] = true;
            
            teams[i].index = i;
            teams[i].TeamUpdated += OnTeamUpdated;
        }
    }

    private void OnTeamUpdated(short teamindex, int membercount)
    {
        if (membercount == 0)
        {
            Debug.Log("Game over");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void MatchBegin()
    {
        _activeTeam = 0;
        foreach (Team t in teams)
        {
            t.TurnEnded += OnturnEnded;
        }

        MatchStarted?.Invoke(this, EventArgs.Empty);
        Turn();
    }

    private void Turn()
    {
        cinemachineVirtualCamera.Follow = teams[_activeTeam].GetActivePlayer().transform; 
        // Give control to the next character of the active player
        
        // Wait for player action
        teams[_activeTeam].PlayerAction();

        // follow first projectile if there is one
        ProjectileWeapon pw = teams[_activeTeam].GetActivePlayer().weapon as ProjectileWeapon;
        if (pw != null) pw.ProjectileFired += OnProjectileFired;
    }

    private void OnProjectileFired(GameObject projectile)
    {
        if (_trackingProjectile) return;
        _trackingProjectile = true;
        cinemachineVirtualCamera.Follow = projectile.transform;
    }

    private void OnturnEnded(object sender, EventArgs e)
    {
        _trackingProjectile = false;
        _activeTeam = (_activeTeam == 1) ? 0 : 1;
        Debug.Log("wait for next turn");
        Invoke(nameof(Turn), 1f);
    }
}
