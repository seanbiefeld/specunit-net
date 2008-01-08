using System;
using NUnit.Framework;
using SpecUnit;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[TestFixture]
	[Concern(typeof(Context))]
	public class when_a_test_fixture_class_is_a_concern
	{
		[Observation]
		public void should_be_attributed_with_the_ConcernAttribute()
		{
			Context.HasConcern(typeof(Context_with_concern)).ShouldBeTrue();
		}

		[Observation]
		public void should_have_a_name_that_is_prefixed_with_the_concern_name()
		{
			Context context = new Context(typeof(Context_with_concern));
			context.Name.ShouldStartWith("SomeConcern, ");
		}

		[Observation]
		public void should_have_a_concern_name_that_is_the_name_specified_by_the_concern_attribute()
		{
			Context context = new Context(typeof(Context_with_concern));
			context.ConcernName.ShouldEqual("SomeConcern");
		}
	}

	[Concern(typeof(Context))]
	[TestFixture]
	public class when_a_test_fixture_class_is_not_a_concern
	{
		[Observation]
		public void should_not_be_attributed_with_the_ConcernAttribute()
		{
			Context.HasConcern(typeof(Test_fixture_with_underscores)).ShouldBeFalse();
		}

		[Observation]
		public void should_not_have_a_name_that_is_prefixed_with_the_concern_name()
		{
			Context context = new Context(typeof(Test_fixture_with_underscores));
			context.Name.ShouldEqual("Test fixture with underscores");
		}

		[Observation]
		public void should_not_have_a_concern_name()
		{
			Context context = new Context(typeof(Test_fixture_with_underscores));
			context.ConcernName.ShouldBeNull();
		}
	}

	[TestFixture]
	[Concern(typeof(Context))]
	public class when_building_a_context_for_a_test_fixture_type : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = SpecUnit.Report.Context.Build(typeof(Context_with_concern));
		}

		[Test]
		[Observation]
		public void should_collect_and_build_the_specifications()
		{
			_context.Specifications.Length.ShouldEqual(3);
		}

		[Test]
		[Observation]
		public void should_create_a_specification_for_each_test_method_in_the_type()
		{
			_context.HasSpecificationFor("should_jump").ShouldBeTrue();
			_context.HasSpecificationFor("should_jump_when_I_say_how_high").ShouldBeTrue();
			_context.HasSpecificationFor("should_jump_if_I_say_how_high").ShouldBeTrue();
		}

	}

	[Concern(typeof(Context))]
	public class when_creating_a_context_for_a_type_that_is_not_a_test_fixture : ContextSpecification
	{
		[Observation]
		public void should_cause_an_error()
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
		public void they_should_be_equal()
		{
			_contextClass1.ShouldEqual(_contextClass2);
		}
	}

	[TestFixture]
	[Concern(typeof(Context))]
	public class when_a_context_behaves_like_another_context : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = SpecUnit.Report.Context.Build(typeof(Context_with_behaves_like));
		}

		[Test]
		[Observation]
		public void should_have_a_subclass_whose_name_begins_with__behaves_like__()
		{
			_context.BehavesLike.ShouldEqual("a common context");
		}
	}

	[TestFixture]
	[Concern(typeof(Context))]
	public class when_a_context_does_not_behave_like_another_context : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = SpecUnit.Report.Context.Build(typeof(Context_with_concern));
		}

		[Test]
		[Observation]
		public void should_not_have_a_subclass_whose_name_begins_with__behaves_like__()
		{
			_context.BehavesLike.ShouldBeNull();
		}

		[Test]
		[Observation]
		public void may_not_have_a_subclass()
		{
			SpecUnit.Report.Context.Build(typeof(Context_with_subclass)).BehavesLike.ShouldBeNull();
		}
	}
}