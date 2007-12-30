using NUnit.Framework;

namespace SpecUnit.Specs.AssemblyUnderTest
{
    [Concern(typeof(SomeConcern))]
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

    [Concern(typeof(SomeConcern))]
    public class Context_with_same_concern
    {
        [Test]
        public void should_quack()
        {

        }
    }

    [Concern(typeof(SomeOtherConcern))]
    public class Context_with_some_other_concern
    {
        [Test]
        public void should_walk()
        {

        }
    }
}