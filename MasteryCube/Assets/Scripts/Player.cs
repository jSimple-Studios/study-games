using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : NetworkBehaviour
{
    GameManager gm;
    [SyncVar] public string username;
    [SyncVar] [SerializeField] bool runningGame;
    [SyncVar] int score;
    [SyncVar] int activeQID;
    [SyncVar] public TimeSpan time;
    void Start() {
        gm = FindObjectOfType<GameManager>();
        CmdRegisterNew(this, gm.playerName.text);
    }

    void Update() {
        if (runningGame) {
            // update timer & score
            gm.time = time;
            gm.infotext.text = "Score: " + score.ToString();
            
            // play pending anims
        }
    }

    [Command(requiresAuthority = false)] public void CmdRegisterNew(Player player, string username) {
        // all refs to gm here are on server only, doesnt propagate to clients
        player.username = username;
        gm.players.Add(player);
        var newListing = (GameObject)Resources.Load("Prefabs/PlayerListItem");
        newListing.GetComponent<PlayerListItem>().playerName.text = username;
        Instantiate(newListing);
    }

    [ClientRpc] public void StartGame() {
        runningGame = true;
        // StartCoroutine(EStartGame());
    }

    IEnumerator EStartGame() {
        // request q from server
        // lerp to q
        // wait for player input
        // send input to server
        // update score based on correct or not
        return null;
    }

    [ClientRpc] public void StopGame() {
        runningGame = false;
        // hide game ui and go to end screen
        // show personal score
    }
}
