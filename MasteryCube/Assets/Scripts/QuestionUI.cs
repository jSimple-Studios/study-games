using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Org.BouncyCastle.Crypto.Macs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour {
    public TMP_Text qTXT;
    public Button[] ansBTNs;
    int selectedBtn = -1;

    public int AskQuestion(int qid) {
        // load vals
        // gameObject.SetActive(true);
        GameManager gm = FindObjectOfType<GameManager>();
        Question question = gm.set.set[qid];
        print(gm.set.set[qid].question);
        print(qid);
        selectedBtn = -1;
        qTXT.text = question.question;
        for (int i = 0; i < ansBTNs.Length; i++) {
            ansBTNs[i].GetComponentInChildren<TMP_Text>().text = question.responses[i].text;
        }

        // wait for answer
        while (selectedBtn == -1) {}

        // end
        gameObject.SetActive(false);
        return selectedBtn;
    }

    public void SelectBtn(int id) {
        selectedBtn = id;
    }
}
