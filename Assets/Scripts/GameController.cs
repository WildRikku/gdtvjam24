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
    private bool _ignoreTimer; // use to not end turn because of grenade timers

    public List<string> botNames; // use in PlayerController
    public List<string> matchStartMessages;

    private void Awake() {
        Invoke(nameof(MatchBegin), 4f);
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
        menuController.WeaponChangedByUser += OnWeaponChangedByUser;
    }

    private void OnWeaponChangedByUser(int teamWeaponIndex) {
        teams[activeTeam].GetActivePlayer().ChangeWeapon(teams[activeTeam].weaponPrefabs[teamWeaponIndex]);
    }

    private void OnTurnTimeIsUp(object sender, EventArgs e) {
        if (_ignoreTimer) {
            return;
        }

        teams[activeTeam].EndTurn(true);
        EndTurn();
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
            t.TurnEnded += OnTeamTurnEnded;
        }

        MatchStarted?.Invoke(this);
        menuController.TriggerMessage("Referee: " + matchStartMessages[Random.Range(0,matchStartMessages.Count)], 4);
        Turn();
    }

    private void Turn() {
        _ignoreTimer = false;
        TurnStarted?.Invoke(this);

        // Activate next player
        teams[activeTeam].PlayerAction();
        AudioManager.Instance.PlaySFX("CameraSwipe");
        cinemachineVirtualCamera.Follow = teams[activeTeam].GetActivePlayer().transform;

        // follow first projectile if there is one
        ProjectileWeapon pw = teams[activeTeam].GetActivePlayer().weapon as ProjectileWeapon;
        if (pw != null) {
            pw.ProjectileFired += OnProjectileFired;
        }
    }

    private void OnProjectileFired(GameObject projectile) {
        _ignoreTimer = true;
        menuController.HideWeaponHUD();

        if (_trackingProjectile) {
            return;
        }

        _trackingProjectile = true;
        cinemachineVirtualCamera.Follow = projectile.transform;
    }

    private void OnTeamTurnEnded(object sender, EventArgs e) {
        EndTurn();
    }

    private void EndTurn() {
        _trackingProjectile = false;
        activeTeam = activeTeam == 1 ? 0 : 1;
        Debug.Log("wait for next turn, next up is team " + activeTeam);
        Invoke(nameof(Turn), 1f);
    }
}