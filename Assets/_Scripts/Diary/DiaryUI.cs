using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : CanvasController
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _entryPrefab;

    [Header("Selection Settings")]
    [SerializeField] private TMP_Text _entryName;
    [SerializeField] private TMP_Text _entryDescription;
    [SerializeField] private TMP_Text _entryHighestWeight;
    [SerializeField] private TMP_Text _entryTimesCaught;
    [SerializeField] private Image _entryImage;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ToggleDiary();
    }
    
    private void Start()
    {
        DiaryManager.Instance.OnEntriesLoaded += BindDatas;
    }

    private void BindDatas(HashSet<EntryData> entries)
    {
        foreach (EntryData data in entries)
        {
            DiaryEntryUI entry = Instantiate(_entryPrefab, _contentParent).GetComponent<DiaryEntryUI>();
            entry.BindData(data);
        }
    }

    private void OpenDiary()
    {
        ShowCanvas();
    }

    private void CloseDiary()
    {
        HideCanvas();
    }

    private void ToggleDiary()
    {
        if (IsOpen)
            CloseDiary();
        else
            OpenDiary();
    }

    public void SelectEntry(EntryData entry)
    {
        _entryImage.sprite = entry.FishData.Icon;
        _entryName.text =  entry.FishData.DisplayName;
        _entryDescription.text =  entry.FishData.Description;
        _entryTimesCaught.text = $"Times Caught: {entry.TimesCaught}";
        if (entry.HighestWeight > 1f)
        {
            _entryHighestWeight.text = $"Heaviest Caught: {entry.HighestWeight}kg";
        }
        else
        {
            _entryHighestWeight.text = $"Heaviest Caught: {entry.HighestWeight * 100}g";
        }
    }
}