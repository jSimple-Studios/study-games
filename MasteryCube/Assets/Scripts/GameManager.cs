using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour {
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
    [SyncVar] public List<Player> players = new();
    int numQs;

    void Start() {
        set = JsonUtility.FromJson<Set>(questionsJson.text);
        numQs = set.set.Length;

        #region generate cube
        // div by 6 then sqrt, rounding up to the next whole number
        int qsPerCol = Mathf.CeilToInt(Mathf.Sqrt(numQs/6));
        List<CubeSegment> segments = new();
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
                    segments.Add(curseg);
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

    void Update() {
        if (hostingOnly) {
            serverCam.RotateAround(Vector3.zero, new Vector3(1,1,0), spinSpeed * Time.deltaTime);
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
        nm.networkAddress = serverIP.text;
        nm.StartClient();
    }

    [Command(requiresAuthority = false)] public void CmdRegisterNew(Player player, string username) {
        player.username = username;
        players.Add(player);
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