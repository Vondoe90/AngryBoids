using System;
using CryEngine.Testing;

namespace CryGameCode.Testing
{
	[TestCollection]
	public class TestClass
	{
		[Test]
		public void ReferencesEqual()
		{
			var lhs = new object();
			var rhs = lhs;
			Assert.IsTrue(lhs == rhs);
		}

		[Test]
		public void ReferencesUnequal()
		{
			var lhs = new object();
			var rhs = new object();
			Assert.IsTrue(lhs != rhs);
		}

		[Test]
		public void Catching()
		{
			var obj = new object();

			Assert.Throws<InvalidCastException>(() =>
			{
				var myInt = (int)obj;
			});
		}

		[Test]
		public void False()
		{
			Assert.IsTrue(false);
		}

		/* This will die hard in unmanaged code, CScriptClass::CallMethod, before the exception handler has a chance
		[Test]
		public void NullRef()
		{
			object obj = null;
			obj.GetType();
		}*/
	}
}
