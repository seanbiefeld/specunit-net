using NUnit.Framework;

namespace SpecUnit
{
	[TestFixture]
	public abstract class ContextSpecification
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Context_BeforeAllSpecs();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			CleanUpContext_AfterAllSpecs();
		}

		[SetUp]
		public void SetUp()
		{
			Context();
			Because();
		}

		[TearDown]
		public void TearDown()
		{
			Because_After();
			CleanUpContext();
		}

		protected virtual void Context() { }
		protected virtual void CleanUpContext() { }
		protected virtual void Context_BeforeAllSpecs() { }
		protected virtual void CleanUpContext_AfterAllSpecs() { }
		protected virtual void Because() { }
		protected virtual void Because_After() { }
	}
}
