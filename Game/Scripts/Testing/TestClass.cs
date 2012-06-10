using System;
using CryEngine.Testing;

namespace CryGameCode.Testing
{
	[TestCollection("Reference self testing")]
	public class References
	{
		[Test("Referential equality", "When A is instantiated and B is assigned to A, A and B are referentially equal.")]
		public void ReferencesEqual()
		{
			var lhs = new object();
			var rhs = lhs;
			Assert.IsTrue(lhs == rhs);
		}

		[Test("Referential inequality", "Two separately instantiated objects are not referentially equal.")]
		public void ReferencesUnequal()
		{
			var lhs = new object();
			var rhs = new object();
			Assert.IsTrue(lhs != rhs);
		}
	}

	[TestCollection("Exception self testing")]
	public class Exceptions
	{
		[Test("Assert.Throws", "Should catch exceptions thrown within the lambda.")]
		public void Catching()
		{
			var obj = new object();

			Assert.Throws<InvalidCastException>(() =>
			{
				var myInt = (int)obj;
			});
		}

		[Test("Intentional failure", "This is an example of a test that fails an assertion.")]
		public void False()
		{
			Assert.IsTrue(false);
		}

		// This will die hard in unmanaged code, CScriptClass::CallMethod, before the exception handler has a chance
		// Nice test of IgnoreTestAttribute though!
		[IgnoreTest]
		[Test("Intentionally ignored", "This is an example of a test that isn't executed.")]
		public void NullRef()
		{
			object obj = null;
			obj.GetType();
		}
	}
}
