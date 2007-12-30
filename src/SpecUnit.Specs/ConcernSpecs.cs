using System;
using NUnit.Framework;
using SpecUnit;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[Concern(typeof(Concern))]
	public class when_creating_a_concern_for_a_test_fixture_type : ContextSpecification
	{
		private Concern _concern;

		protected override void Context()
		{
			_concern = new Concern(typeof(SomeConcern));
		}

		[Observation]
		public void should_be_named_for_the_concern_type_s_name()
		{
			_concern.Name.ShouldEqual("SomeConcern");
		}
	}

	[Concern(typeof(Concern))]
	public class after_adding_a_context_for_a_type : ContextSpecification
	{
		private Concern _concern;

		protected override void Context()
		{
			_concern = new Concern(typeof(SomeConcern));
		}

		protected override void Because()
		{
			_concern.AddContextFor(typeof(Context_with_concern));
		}

		[Observation]
		public void should_have_the_context()
		{
			_concern.HasContextFor(typeof(Context_with_concern)).ShouldBeTrue();
		}
	}

	[Concern(typeof(Concern))]
	public class when_a_context_does_not_belong_to_a_concern : ContextSpecification
	{
		private Concern _concern;

		protected override void Context()
		{
			_concern = new Concern(typeof(SomeConcern));
		}

		[Observation]
		public void should_not_have_the_concern()
		{
			_concern.HasContextFor(typeof(Context_with_concern)).ShouldBeFalse();
		}
	}

	[Concern(typeof(Concern))]
	public class when_adding_multiple_contexts_of_the_same_concern_to_a_concern : ContextSpecification
	{
		private Concern _concern;

		protected override void Context()
		{
			_concern = new Concern(typeof(SomeConcern));
		}

		protected override void Because( /* contexts belong to the same concern */ )
		{
			_concern.AddContextFor(typeof(Context_with_concern));
			_concern.AddContextFor(typeof(Context_with_same_concern));
		}

		[Observation]
		public void should_list_the_contexts_that_have_been_added()
		{
			_concern.Contexts.Length.ShouldEqual(2);
			_concern.HasContextFor(typeof(Context_with_concern)).ShouldBeTrue();
			_concern.HasContextFor(typeof(Context_with_same_concern)).ShouldBeTrue();
		}
	}

	[TestFixture]
	[Concern(typeof(Concern))]
	public class when_adding_a_context_to_a_concern_where_the_context_does_not_belong_to_the_concern : ContextSpecification
	{
		private Concern _concern;
		private Type _testFixtureType;
		private MethodThatThrows _method;
		private Exception _exception;

		protected override void Context()
		{
			_concern = new Concern(typeof(Context_with_concern).GetConcernName());
			_concern.AddContextFor(typeof(Context_with_concern));

			_testFixtureType = typeof(Context_with_some_other_concern);
		}

		protected override void Because()
		{
			_method = delegate
			{
				_concern.AddContextFor(_testFixtureType);
			};

			_exception = _method.GetException();
		}

		[Test]
		[Observation]
		public void should_not_be_allowed()
		{
			_exception.ShouldContainErrorMessage(String.Format("{0} does not belong to the {1} concern.", _testFixtureType.Name, "SomeConcern"));
		}
	}
}