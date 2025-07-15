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

    private QuestProgress activeQuest;
    
    private void Awake()
    {
        this.gameObject.SetActive(false);
        QuestManager.OnStartQuest.AddListener(Description);
        QuestManager.OnFinishQuest.AddListener(EndQuest);

        description.text = "";
        //title.text = "";
        objectiveDescription.text = "";
    }

    private void Description(QuestProgress quest)
    {
        activeQuest = quest;
        this.gameObject.SetActive(true);

        var questData = quest.QuestData;
        description.text = questData.description;
        //title.text = questData.title;
        objectiveDescription.text = questData.objectiveDescription;
    }
    private void EndQuest()
    {
        this.gameObject?.SetActive(false);
    }
}
