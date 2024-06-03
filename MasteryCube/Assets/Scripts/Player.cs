using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar] public string username;
    GameManager gm;
    void Start() {
        gm = FindObjectOfType<GameManager>();
        CmdRegisterNew(this, gm.playerName.text);
    }

    void Update() {

    }

    [Command(requiresAuthority = false)] public void CmdRegisterNew(Player player, string username) {
        player.username = username;
        gm.players.Add(player);
    }
}
