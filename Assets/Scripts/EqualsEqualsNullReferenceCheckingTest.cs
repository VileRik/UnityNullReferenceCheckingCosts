using UnityEngine;

public class EqualsEqualsNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public EqualsEqualsNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "==";
    protected override bool PerformNullReferenceCheck()
    {
        return gameObjectToCheckAgainst == null;
    }
}