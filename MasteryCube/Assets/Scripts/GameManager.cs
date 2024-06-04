using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public bool hostingOnly;
    public TextAsset questionsJson;
    public Transform theCube;
    public Set set;
    public Transform serverCam;
    public float spinSpeed = 15;
    public GameObject titleUI;
    public GameObject serverUI;
    public GameObject clientUI;
    public TMP_InputField serverIP;
    public TMP_InputField playerName;
    [SerializeField] NetMan nm;
    public List<Player> players = new();
    public GameObject playerList;
    public List<CubeSegment> segs;
    public List<int> activeQs;
    public List<int> finishedQs;
    public bool runningGame;
    public TMP_Text infotext;
    public TMP_Text svrinfotext;
    public TimeSpan time;
    public QuestionUI qui;
    float curTime;
    int numQs;

    void Start() {
        set = JsonUtility.FromJson<Set>(questionsJson.text);
        numQs = set.set.Length;

        #region generate cube
        // div by 6 then sqrt, rounding up to the next whole number
        int qsPerCol = Mathf.CeilToInt(Mathf.Sqrt(numQs/6));
        List<GameObject> sides = new();
        for (int i2 = 0; i2 < 6; i2++) {
            sides.Add(new GameObject());
            sides[i2].transform.parent = theCube;
            for (int i1 = 0; i1 < qsPerCol; i1++) {
                for (int i = 0; i < qsPerCol; i++) {
                    CubeSegment curseg = Instantiate(Resources.Load("Prefabs/segment"), sides[i2].transform).GetComponent<CubeSegment>();
                    curseg.col = i;
                    curseg.row = i1;
                    curseg.SetPos();
                    segs.Add(curseg);
                    curseg.id = segs.IndexOf(curseg);
                }
            }
            
            sides[i2].transform.localPosition -= (qsPerCol-1f) * Vector3.left;
            sides[i2].transform.localPosition -= (qsPerCol-1f) * Vector3.forward;
            sides[i2].transform.RotateAround(Vector3.zero, Vector3.left, 180);
            sides[i2].transform.localPosition += qsPerCol * Vector3.down;
        }
        sides[0].transform.RotateAround(Vector3.zero, Vector3.left, 180);

        sides[1].transform.RotateAround(Vector3.zero, Vector3.left, 90);

        sides[2].transform.RotateAround(Vector3.zero, Vector3.left, 90);
        sides[2].transform.RotateAround(Vector3.zero, Vector3.up, 90);

        sides[3].transform.RotateAround(Vector3.zero, Vector3.left, 90);
        sides[3].transform.RotateAround(Vector3.zero, Vector3.up, 180);

        sides[4].transform.RotateAround(Vector3.zero, Vector3.left, 90);
        sides[4].transform.RotateAround(Vector3.zero, Vector3.up, 270);
        #endregion

    }

    void FixedUpdate() {
        if (hostingOnly) {
            serverCam.RotateAround(Vector3.zero, new Vector3(1,1,0), spinSpeed * Time.deltaTime);
            for (int i = 0; i < players.Count; i++) {
                if (players[i] == null) {
                    foreach (var item in playerList.GetComponentsInChildren<PlayerListItem>())
                        if (item.playerName.text == players[i].username) Destroy(item.gameObject);
                    players.RemoveAt(i);
                }
            }
        }
    }

    void Update() {
        if (hostingOnly && runningGame) {
            // stopwatch
            curTime += Time.deltaTime;
            time = TimeSpan.FromSeconds(curTime);
            svrinfotext.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();

            // change score on list items
            foreach (var item in playerList.GetComponentsInChildren<PlayerListItem>()) {
                foreach (var player in players) {
                    if (player.username == item.playerName.text) {
                        item.playerScore.text = player.score.ToString();
                    }
                }
            }
        }
    }

    public void SetSpinSpeed(Slider slider) {
        spinSpeed = slider.value;
    }

    public void OpenServerMode() {
        hostingOnly = true;
        titleUI.SetActive(false);
        clientUI.SetActive(false);
        serverUI.SetActive(true);
        nm.StartServer();
    }

    public void JoinServer() {
        if (serverIP.text != "" && playerName.text != "") {
            nm.networkAddress = serverIP.text;
            nm.StartClient();
            clientUI.SetActive(true);
            titleUI.SetActive(false);
        }
    }

    public void StartGame() {
        runningGame = true;
        foreach (var player in players) {
            player.StartGame();
        }
        titleUI.SetActive(false);
        serverUI.SetActive(true);
    }

    public int ReqQuestion() {
        // give the player a question that isn't already completed or being used
        int _id = -1;
        bool _ready = false;
        while (_id != -1 && !_ready) {
            _id = UnityEngine.Random.Range(0, set.set.Length);
            if (!activeQs.Contains(_id) && !finishedQs.Contains(_id))
                _ready = true;
        }

        return _id;
    }
}

[Serializable] public struct Response {
    public string text;
    public bool accept;
}

[Serializable] public class Question {
    public string question;
    public Response[] responses;
}

[Serializable] public class Set {
    public Question[] set;
}