using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class NetMan : NetworkManager {
    [SerializeField] GameManager gm;

    public override void Start() {
        base.Start();
        gm.gameObject.SetActive(true);
    }
}
