using UnityEngine;

public class IsNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public IsNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "is";
    protected override bool PerformNullReferenceCheck()
    {
        return gameObjectToCheckAgainst is null;
    }
}