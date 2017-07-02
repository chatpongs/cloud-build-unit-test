using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class UnitTest : MonoBehaviour {

	[Test]
	public static void AlwaysPass() 
    {
        Assert.That(true);
	}
}
