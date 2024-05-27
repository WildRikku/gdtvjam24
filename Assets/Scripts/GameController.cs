using System.Collections.Generic;
using DTerrain;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BasicPaintableLayer collidableLayer;
    
    public List<Team> Teams;

    private int _activeTeam;

    private void Start()
    {
        Invoke(nameof(MatchBegin), 2f);
    }

    void MatchBegin()
    {
        _activeTeam = 1;
        Turn();
    }

    void Turn()
    {
        // Give control to the next character of the active player
        
        // Wait for player action
        Teams[_activeTeam].PlayerAction();
        
        Invoke(nameof(TurnEnded), 5f);
    }

    void TurnEnded()
    {
        _activeTeam = (_activeTeam == 1) ? 2 : 1;
        Turn();
    }
}
