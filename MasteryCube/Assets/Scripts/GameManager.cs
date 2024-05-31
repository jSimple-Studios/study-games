using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public TextAsset questionsJson;
    public Question[] set;

    void Start() {
        // generate cube
    }

    void Update() {
        
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