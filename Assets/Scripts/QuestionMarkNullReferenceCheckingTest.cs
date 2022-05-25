using UnityEngine;

public class QuestionMarkNullReferenceCheckingTest : NullReferenceCheckingTest
{
    public QuestionMarkNullReferenceCheckingTest(int timesToCheck, GameObject gameObjectToCheckAgainst) : base(timesToCheck, gameObjectToCheckAgainst) {}

    public override string Name => "?";
    protected override bool PerformNullReferenceCheck()
    {
        return gameObjectToCheckAgainst ? true : false;
    }
}