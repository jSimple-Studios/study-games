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
        if (isLocalPlayer) {
            gm.CmdRegisterNew(this, gm.playerName.text);
        }
    }

    void Update() {

    }
}
