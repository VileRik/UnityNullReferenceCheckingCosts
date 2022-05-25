using TMPro;
using UnityEngine;

public sealed class TestResultsReferenceContainer : MonoBehaviour
{
    [SerializeField] private RectTransform bar;
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI counterDisplay;
    [SerializeField] private TextMeshProUGUI timeDisplay;

    public RectTransform Bar => bar;
    public TextMeshProUGUI NameDisplay => nameDisplay;
    public TextMeshProUGUI CounterDisplay => counterDisplay;
    public TextMeshProUGUI TimeDisplay => timeDisplay;

}
