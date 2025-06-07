using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishQuestHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI congratulations;


    private void Awake()
    {
        this.gameObject.SetActive(false);
        QuestManager.OnFinishQuest.AddListener(MissionAcomplished);

        congratulations.text = "";
    
    }

    public void MissionAcomplished()
    {
        this.gameObject.SetActive(true);
        congratulations.text = "Missão Concluída";
    }
}

