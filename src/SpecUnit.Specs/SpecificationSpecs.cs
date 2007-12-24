using System;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;

namespace SpecUnit.Specs
{
	[Concern(typeof(Specification))]
	public class when_a_method_has_a_test_fixture_attribute : ContextSpecification
	{
		[Observation]
		public void should_be_a_test_method()
		{
			MethodInfo methodInfo = (MethodInfo)MethodInfo.GetCurrentMethod();

			Specification.IsTestMethod(methodInfo).ShouldBeTrue();
		}
	}

	[Concern(typeof(Specification))]
	public class when_a_specification_name_contains__when__ : ContextSpecification
	{
		private Specification specification;

		protected override void Because(/* the specification name includes "when" */)
		{
			specification = new Specification("should_ask_how_high_when_I_say_jump");
		}

		[Observation]
		public void should_add_a_warning_to_the_specification_name_indicating_that_the_specification_may_be_a_context()
		{
			specification.Name.ShouldContain(" -- WARNING: Specifications with the words \"if\" or \"when\" may be contexts");
		}
	}

	[Concern(typeof(Specification))]
	public class when_a_specification_name_contains__if__ : ContextSpecification
	{
		private Specification specification;

		protected override void Because(/* the specification name includes "if" */)
		{
			specification = new Specification("should_ask_how_high_if_I_say_jump");
		}

		[Observation]
		public void should_add_a_warning_to_the_specification_name_indicating_that_the_specification_may_be_a_context()
		{
			specification.Name.ShouldContain(" -- WARNING: Specifications with the words \"if\" or \"when\" may be contexts");
		}
	}
}