using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour {
    public TMP_Text playerName;
    public TMP_Text playerScore;
    void Start() {
        transform.parent = FindObjectOfType<GameManager>().playerList.transform;
    }
}
