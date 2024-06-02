using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CubeSegment : NetworkBehaviour {
    public int row;
    public int col;
    public CubeState state;
    public Material[] mats;
    Renderer rend;

    void Start(){
        rend = GetComponentInChildren<Renderer>();
    }
    public void SetPos() {
        transform.localPosition += 2 * col * Vector3.left;
        transform.localPosition += 2 * row * Vector3.forward;
    }

    void Update() {
        rend.material = mats[(int)state];
    }
}

public enum CubeState { Pending, Correct, Incorrect }