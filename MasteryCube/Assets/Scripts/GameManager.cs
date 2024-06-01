using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public TextAsset questionsJson;
    public Question[] set;
    public int numQs; // this is a temporary

    void Start() {
        // generate cube
            // div by 6 then sqrt, rounding to nearest whole number
            int qsPerCol = numQs/6;
            
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