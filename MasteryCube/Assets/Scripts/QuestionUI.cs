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
        ansBTNs[0].GetComponentInChildren<TMP_Text>().text = question.responses[0].text;
        ansBTNs[1].GetComponentInChildren<TMP_Text>().text = question.responses[1].text;
        ansBTNs[2].GetComponentInChildren<TMP_Text>().text = question.responses[2].text;
        ansBTNs[3].GetComponentInChildren<TMP_Text>().text = question.responses[3].text;


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
