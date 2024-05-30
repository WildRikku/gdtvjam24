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
            teams[i].index = i;
            if (menuController != null)
            {
                menuController.teamColorIndex[i] = teamColor;
            }

            usedTeamColors[teamColor] = true;
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
    }

    private void OnturnEnded(object sender, EventArgs e)
    {
        _activeTeam = (_activeTeam == 1) ? 0 : 1;
        Turn();
    }
}
