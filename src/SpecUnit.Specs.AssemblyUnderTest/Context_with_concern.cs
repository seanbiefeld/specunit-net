using NUnit.Framework;

namespace SpecUnit.Specs.AssemblyUnderTest
{
	[Concern(typeof(SomeConcern))]
	[TestFixture]
	public class Context_with_concern
	{
		[Test]
		public void should_jump()
		{
			
		}

		[Test]
		public void should_jump_when_I_say_how_high()
		{
			
		}

		[Test]
		public void should_jump_if_I_say_how_high()
		{

		}
	}
}