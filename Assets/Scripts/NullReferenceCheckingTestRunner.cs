using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public sealed class NullReferenceCheckingTestRunner : MonoBehaviour
{
    [Min(1)][SerializeField] private int updatesToRun = 100000;
    [Min(1)][SerializeField] private int checksPerUpdate = 100000;
    [Min(0)][SerializeField] private float startupDelayInSeconds = 5;
    
    [Header("Tests To Run")]
    [SerializeField] private bool measureEqualsEquals = true;
    [SerializeField] private bool measureIs = true;
    [SerializeField] private bool measureReferenceEquals = true;
    [SerializeField] private bool measureQuestionMark = true;
    
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
        if (measureEqualsEquals)
            _testsToDo.Enqueue(new EqualsEqualsNullReferenceCheckingTest(updatesToRun, gameObject));
        
        if (measureIs)
            _testsToDo.Enqueue(new IsNullReferenceCheckingTest(updatesToRun, gameObject));
        
        if (measureReferenceEquals)
            _testsToDo.Enqueue(new ReferenceEqualsNullReferenceCheckingTest(updatesToRun, gameObject));
        
        if (measureQuestionMark)
            _testsToDo.Enqueue(new QuestionMarkNullReferenceCheckingTest(updatesToRun, gameObject));
        
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
        {
            _stopwatch.Stop();
            _activeTestResultsReferenceContainer.TimeDisplay.text = $"{_stopwatch.ElapsedMilliseconds} ms";
            _activeTest = null;
        }
        
        if (_activeTest is null)
        {
            if (!TryStartNextTest())
            {
                _finished = true;
                return;
            }
        }
        
        _activeTest.Check(checksPerUpdate);
        _activeTestResultsReferenceContainer.CounterDisplay.text = _activeTest.timesChecked.ToString();
        var barAnchorMaxY = _activeTest.timesChecked / (float)updatesToRun;
        _activeTestResultsReferenceContainer.Bar.anchorMax = new Vector2(1, barAnchorMaxY);
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
        else return false;
    }
}

public abstract class NullReferenceCheckingTest
{
    private int timesToCheck;
    
    public int timesChecked { get; private set; }
    public bool finished => timesChecked >= timesToCheck;
    
    protected GameObject gameObjectToCheckAgainst;
    
    public NullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst)
    {
        this.timesToCheck = timesToCheck;
        this.gameObjectToCheckAgainst = gameObjectToCheckAgainst;
    }
    
    public void Check(int numberOfChecks)
    {
        if (timesChecked + numberOfChecks > timesToCheck)
            numberOfChecks = timesToCheck - timesChecked;
        
        for (var i = 0; i < numberOfChecks; i++)
            if (PerformNullReferenceCheck()) {}
        
        timesChecked += numberOfChecks;
    }

    public abstract string Name { get; }
    protected abstract bool PerformNullReferenceCheck();
}

public class EqualsEqualsNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public EqualsEqualsNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "==";
    protected override bool PerformNullReferenceCheck()
    {
        return gameObjectToCheckAgainst == null;
    }
}

public class IsNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public IsNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "is";
    protected override bool PerformNullReferenceCheck()
    {
        return gameObjectToCheckAgainst is null;
    }
}

public class ReferenceEqualsNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public ReferenceEqualsNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "ReferenceEquals";
    protected override bool PerformNullReferenceCheck()
    {
        return ReferenceEquals(gameObjectToCheckAgainst, null);
    }
}

public class QuestionMarkNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public QuestionMarkNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "?";
    protected override bool PerformNullReferenceCheck()
    {
        return gameObjectToCheckAgainst ? true : false;
    }
}