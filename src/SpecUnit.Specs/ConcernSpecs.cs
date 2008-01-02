using System;
using NUnit.Framework;
using SpecUnit;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[Concern(typeof(Concern))]
	public class when_creating_a_concern_based_on_a_type : ContextSpecification
	{
		private Concern _concern;

		protected override void Context()
		{
			_concern = new Concern(typeof(SomeConcern));
		}

		[Observation]
		public void should_be_named_for_the_type()
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
		public void should_have_the_context_in_its_list_of_contexts()
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
		public void should_not_have_the_context_in_its_list_of_contexts()
		{
			_concern.HasContextFor(typeof(Context_with_concern)).ShouldBeFalse();
		}
	}

	[TestFixture]
	[Concern(typeof(Concern))]
	public class when_adding_a_context_to_a_concern_that_it_does_not_belong_to : ContextSpecification
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
		public void should_cause_an_error()
		{
			_exception.ShouldContainErrorMessage(String.Format("{0} does not belong to the {1} concern.", _testFixtureType.Name, "SomeConcern"));
		}
	}
}