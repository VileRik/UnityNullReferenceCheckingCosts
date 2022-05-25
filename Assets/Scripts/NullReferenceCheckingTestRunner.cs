using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public sealed class NullReferenceCheckingTestRunner : MonoBehaviour
{
    [Min(1)][SerializeField] private int updatesToRun;
    [Min(1)][SerializeField] private int checksPerUpdate;
    [Min(0)][SerializeField] private float startupDelayInSeconds;
    
    [Header("Tests To Run")]
    [SerializeField] private bool measureEqualsEquals;
    [SerializeField] private bool measureIs;
    [SerializeField] private bool measureReferenceEquals;
    [SerializeField] private bool measureQuestionMark;
    
    [Header("Game Object References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject testResultPrefab;

    private readonly Queue<NullReferenceCheckingTest> _testsToDo = new Queue<NullReferenceCheckingTest>();
    private NullReferenceCheckingTest _activeTest;
    
    private readonly Queue<TestResultsReferenceContainer> _testResultsReferenceContainers = new Queue<TestResultsReferenceContainer>();
    private TestResultsReferenceContainer _activeTestResultsReferenceContainer;
    
    private bool _finished;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    
    void Start()
    {
        EnqueueTestsToDo();
        EnqueueTestResultsReferenceContainers();
    }

    private void EnqueueTestsToDo()
    {
        if (measureEqualsEquals)
            _testsToDo.Enqueue(new EqualsEqualsNullReferenceCheckingTest(updatesToRun, gameObject));
        
        if (measureIs)
            _testsToDo.Enqueue(new IsNullReferenceCheckingTest(updatesToRun, gameObject));
        
        if (measureReferenceEquals)
            _testsToDo.Enqueue(new ReferenceEqualsNullReferenceCheckingTest(updatesToRun, gameObject));
        
        if (measureQuestionMark)
            _testsToDo.Enqueue(new QuestionMarkNullReferenceCheckingTest(updatesToRun, gameObject));
    }

    private void EnqueueTestResultsReferenceContainers()
    {
        for (var i = 0; i < _testsToDo.Count; i++)
        {
            var testResultsReferenceContainer = Instantiate(testResultPrefab, canvas.transform).GetComponent<TestResultsReferenceContainer>();
            _testResultsReferenceContainers.Enqueue(testResultsReferenceContainer);
        }
    }

    void Update()
    {
        if (_finished || Time.realtimeSinceStartup < startupDelayInSeconds)
            return;

        if (_activeTest?.finished == true)
            EndTest();
        
        if (_activeTest is null && !TryStartNextTest())
        {
            _finished = true;
            return;
        }
        
        _activeTest.Check(checksPerUpdate);
        UpdateActiveTestResults();
    }

    private void EndTest()
    {
        _stopwatch.Stop();
        _activeTestResultsReferenceContainer.TimeDisplay.text = $"{_stopwatch.ElapsedMilliseconds} ms";
        _activeTest = null;
    }

    private bool TryStartNextTest()
    {
        if (_testsToDo.TryDequeue(out var nextTest))
        {
            _activeTest = nextTest;
            _activeTestResultsReferenceContainer = _testResultsReferenceContainers.Dequeue();
            _activeTestResultsReferenceContainer.NameDisplay.text = _activeTest.Name;
            _stopwatch.Restart();
            return true;
        }
        
        return false;
    }

    private void UpdateActiveTestResults()
    {
        _activeTestResultsReferenceContainer.CounterDisplay.text = _activeTest.timesChecked.ToString();
        var barAnchorMaxY = _activeTest.timesChecked / (float)updatesToRun;
        _activeTestResultsReferenceContainer.Bar.anchorMax = new Vector2(1, barAnchorMaxY);
    }
}