using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryEntryUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    private Button _button;
    private DiaryUI _diaryUI;
    private EntryData _entryData;

    private void UpdateEntry()
    {
        _icon.sprite = _entryData.FishData.Icon;
        
        if (_entryData.IsDiscovered)
        {
            _icon.color = Color.white;
            _nameText.text = _entryData.FishData.DisplayName;
            _button.interactable = true;
            
        }
        else
        {
            _icon.color = Color.black;
            _nameText.text = "???";
            _button.interactable = false;
        }
    }

    public void BindData(EntryData data)
    {
        _entryData = data;
        _entryData.OnEntryUpdated += UpdateEntry;
        UpdateEntry();
    }

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectEntry);
        _diaryUI = GetComponentInParent<DiaryUI>();
    }

    private void SelectEntry() => _diaryUI.SelectEntry(_entryData);
}