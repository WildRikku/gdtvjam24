using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public GameObject playerPrefab;
    private List<PlayerController> members;
    private int activeMember;
    
    void Start()
    {
        members = new();
        
        for (short i = 0; i < 5; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab);
            members.Add(newPlayer.GetComponent<PlayerController>());
        }

        activeMember = 0;
    }

    public void PlayerAction()
    {
        members[activeMember].Attack();
        activeMember++;
        if (activeMember == members.Count)
            activeMember = 0;
    }
}
