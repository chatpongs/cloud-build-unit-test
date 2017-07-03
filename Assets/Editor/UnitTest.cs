using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTest {

	[Test]
	public void AlwaysPass() 
	{
		Assert.That(true);
	}
}
