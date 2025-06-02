using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _sliderText;

    private void Start()
    {
        _slider.onValueChanged.AddListener((v) => { _sliderText.text = v.ToString("0");});
    }
}
