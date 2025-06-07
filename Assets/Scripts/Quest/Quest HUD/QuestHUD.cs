using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI objectiveDescription;


    private void Awake()
    {
        this.gameObject.SetActive(false);
        QuestManager.OnStartQuest.AddListener(Description);

        description.text = "";
        title.text = "";
        objectiveDescription.text = "";

        
    }

    private void Description(MyQuestInfo textToWrite)
    {
        this.gameObject.SetActive(true);

        description.text = textToWrite.description;
        title.text = textToWrite.title;
        objectiveDescription.text = textToWrite.objectiveDescription;

    }


}
