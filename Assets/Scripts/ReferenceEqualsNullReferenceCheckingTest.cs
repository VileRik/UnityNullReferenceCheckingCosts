using UnityEngine;

public class ReferenceEqualsNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public ReferenceEqualsNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "ReferenceEquals";
    protected override bool PerformNullReferenceCheck()
    {
        return ReferenceEquals(gameObjectToCheckAgainst, null);
    }
}