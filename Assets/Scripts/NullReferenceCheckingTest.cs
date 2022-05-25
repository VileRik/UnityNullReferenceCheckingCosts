using UnityEngine;

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