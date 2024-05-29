using System;
using System.Collections.Generic;
using Cinemachine;
using DTerrain;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Team> Teams;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    private int _activeTeam;

    private void Start()
    {
        Invoke(nameof(MatchBegin), 2f);
    }

    void MatchBegin()
    {
        _activeTeam = 0;
        for (int i = 0; i < Teams.Count; i++)
        {
            Teams[i].turnEnded += OnturnEnded;
        }

        Turn();
    }

    void Turn()
    {
        cinemachineVirtualCamera.Follow = Teams[_activeTeam].GetActivePlayer().transform; 
        // Give control to the next character of the active player
        
        // Wait for player action
        Teams[_activeTeam].PlayerAction();
    }

    private void OnturnEnded(object sender, EventArgs e)
    {
        _activeTeam = (_activeTeam == 1) ? 0 : 1;
        Turn();
    }
}
