using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void GameplayEvent(GameController gameController);

public class GameController : MonoBehaviour {
    public List<Team> teams;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public GameMenuController menuController;

    public event GameplayEvent MatchStarted;
    public event GameplayEvent TurnStarted;

    public int activeTeam;
    private bool _trackingProjectile;

    public List<string> botNames; // use in PlayerController

    private void Awake() {
        Invoke(nameof(MatchBegin), 2f);
        bool[] usedTeamColors = new bool[4];

        for (short i = 0; i < teams.Count; i++) {
            int teamColor;
            do {
                teamColor = Random.Range(0, 4);
            } while (usedTeamColors[teamColor]);

            teams[i].teamColor = teamColor;
            if (menuController != null) {
                menuController.teamColorIndex[i] = teamColor;
            }

            usedTeamColors[teamColor] = true;

            teams[i].index = i;
            teams[i].TeamUpdated += OnTeamUpdated;
        }

        menuController.TurnTimeIsUp += OnTurnTimeIsUp;
    }

    private void OnTurnTimeIsUp(object sender, EventArgs e) {
        teams[activeTeam].EndTurn(true);
        EndTurn(true);
    }

    private void OnTeamUpdated(short teamindex, int membercount) {
        if (membercount == 0) {
            Debug.Log("Game over");
            menuController.ShowGameEndHUD(teamindex);
/*#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif*/
        }
    }

    private void MatchBegin() {
        activeTeam = 0;
        foreach (Team t in teams) {
            t.TurnEnded += OnTurnEnded;
        }

        MatchStarted?.Invoke(this);
        Turn();
    }

    private void Turn() {
        TurnStarted?.Invoke(this);

        AudioManager.Instance.PlaySFX("CameraSwipe");
        cinemachineVirtualCamera.Follow = teams[activeTeam].GetActivePlayer().transform;
        // Give control to the next character of the active player

        // Wait for player action
        teams[activeTeam].PlayerAction();

        // follow first projectile if there is one
        ProjectileWeapon pw = teams[activeTeam].GetActivePlayer().weapon as ProjectileWeapon;
        if (pw != null) pw.ProjectileFired += OnProjectileFired;
    }

    private void OnProjectileFired(GameObject projectile) {
        menuController.HideWeaponHUD();

        if (_trackingProjectile) return;
        _trackingProjectile = true;
        cinemachineVirtualCamera.Follow = projectile.transform;
    }

    private void OnTurnEnded(object sender, EventArgs e) {
        EndTurn();
    }

    private void EndTurn(bool force = false) {
        _trackingProjectile = false;
        activeTeam = (activeTeam == 1) ? 0 : 1;
        Debug.Log("wait for next turn, next up is team " + activeTeam);
        Invoke(nameof(Turn), 1f);
    }
}