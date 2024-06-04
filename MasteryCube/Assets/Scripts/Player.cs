using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    GameManager gm;
    [SyncVar] public string username;
    [SyncVar] [SerializeField] bool runningGame;
    [SyncVar] public int score;
    [SyncVar] int activeQID;
    [SyncVar] public TimeSpan time;
    Transform cam;
    QuestionUI qui;
    void Start() {
        gm = FindObjectOfType<GameManager>();
        CmdRegisterNew(this, gm.playerName.text);
        cam = FindObjectOfType<Camera>().transform;
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
        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop() {
        while (runningGame) {
            // request q from server
            print ("getting qid");
            GetQid();
            // lerp to cube seg
            print ("lerping to seg");
            for (float i = 0; i < 1.2f; i += Time.deltaTime) {
                cam.position = Vector3.Lerp(cam.position, gm.segs[activeQID].GetComponentInChildren<AudioSource>().transform.position, 2f * Time.deltaTime);
                cam.rotation = Quaternion.Slerp(cam.rotation, gm.segs[activeQID].GetComponentInChildren<AudioSource>().transform.rotation, 2f * Time.deltaTime);
            }
            // wait for player input
            // qui = Instantiate((GameObject)Resources.Load("Prefabs/QuestionUI")).GetComponent<QuestionUI>();
            print ("waiting for inputs");
            qui = gm.qui;
            qui.gameObject.SetActive(true);
            int respID = qui.AskQuestion(activeQID);
            qui.gameObject.SetActive(false);
            // send input to server
            
            // update score based on correct or not

        }
        return null;
    }

    [Command(requiresAuthority = false)] void GetQid() {
        // runs this on the server's version of gm
        activeQID = gm.ReqQuestion();
    }

    [ClientRpc] public void StopGame() {
        runningGame = false;
        // hide game ui and go to end screen
        // show personal score
    }
}
