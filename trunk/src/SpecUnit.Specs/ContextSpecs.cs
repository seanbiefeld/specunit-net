using System;
using NUnit.Framework;
using SpecUnit;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[Concern(typeof(Context))]
	public class when_assigning_a_test_concern_to_a_test_class
	{
		[Observation]
		public void should_indicate_that_it_has_a_concern()
		{
			Context.HasConcern(typeof(Context_with_concern)).ShouldBeTrue();
		}

		[Observation]
		public void should_have_a_concern_name_that_is_the_name_specified_by_the_context_s_concern_attribute()
		{
			Context context = new Context(typeof(Context_with_concern));
			context.ConcernName.ShouldEqual("SomeConcern");
		}

		[Observation]
		public void the_context_name_should_be_prefixed_with_the_concern()
		{
			Context context = new Context(typeof(Context_with_concern));
			context.Name.ShouldStartWith("SomeConcern, ");
		}
	}

	[Concern(typeof(Context))]
	[TestFixture]
	public class when_not_assigning_a_test_concern_to_a_test_class
	{
		[Observation]
		public void should_indicate_that_it_does_not_have_a_concern()
		{
			Context.HasConcern(typeof(Test_fixture_with_underscores)).ShouldBeFalse();
		}

		[Observation]
		public void should_not_have_a_concern_name()
		{
			Context context = new Context(typeof(Test_fixture_with_underscores));
			context.ConcernName.ShouldBeNull();
		}

		[Observation]
		public void should_not_have_a_context_name_that_includes_a_concern()
		{
			Context context = new Context(typeof(Test_fixture_with_underscores));
			context.Name.ShouldEqual("Test fixture with underscores");
		}
	}

	[Concern(typeof(Context))]
	public class when_creating_a_context_for_a_type_that_is_not_a_test_fixture : ContextSpecification
	{
		[Observation]
		public void should_fail()
		{
			typeof(ArgumentException).ShouldBeThrownBy(
				delegate
				{
					new Context(typeof(object));
				}
				)
				.ShouldContainErrorMessage("Object is not a test fixture");
		}
	}

	[Concern(typeof(Context))]
	public class when_creating_a_context_for_a_type_that_has_underscores_in_its_name : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = new Context(typeof(Test_fixture_with_underscores));
		}

		[Observation]
		public void should_remove_the_underscores()
		{
			_context.Name.ShouldEqual("Test fixture with underscores");
		}
	}

	[Concern(typeof(Context))]
	public class when_a_context_class_has_test_methods : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = new Context(typeof(TestFixture));
		}

		[Observation]
		public void should_provide_the_test_methods_as_specifications()
		{
			_context.GetSpecifications().Length.ShouldEqual(2);
		}
	}

	[Concern(typeof(Context))]
	public class when_two_contexts_represent_the_same_test_fixture : ContextSpecification
	{
		private Context _contextClass1;
		private Context _contextClass2;

		protected override void Context()
		{
			_contextClass1 = new Context(typeof(TestFixture));
			_contextClass2 = new Context(typeof(TestFixture));
		}

		[Observation]
		public void should_be_equal()
		{
			_contextClass1.ShouldEqual(_contextClass2);
		}
	}
}