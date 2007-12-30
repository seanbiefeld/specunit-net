using NUnit.Framework;
using SpecUnit.Report;

namespace SpecUnit.Specs
{
	[Concern(typeof(SpecificationName))]
	public class when_the_name_has_underscores : ContextSpecification
	{
		private string _text;
		private string _specificationName;

		protected override void Context()
		{
			_text = "replace_underscores_with_spaces";
		}

		protected override void Because()
		{
			_specificationName = SpecificationName.GetName(_text);
		}

		[Observation]
		public void should_replace_underscores_with_spaces()
		{
			_specificationName.ShouldEqual("replace underscores with spaces");
		}
	}

	[Concern(typeof(SpecificationName))]
	public class when_the_name_has_a_leading_underscore : ContextSpecification
	{
		private string _text;
		private string _specificationName;

		protected override void Context()
		{
			_text = "_starts_with_underscore";
		}

		protected override void Because()
		{
			_specificationName = SpecificationName.GetName(_text);
		}

		[Observation]
		public void should_replace_underscores_with_spaces()
		{
			_specificationName.ShouldEqual("starts with underscore");
		}
	}

	[Concern(typeof(SpecificationName))]
	public class when_the_name_has_a_trailing_underscore : ContextSpecification
	{
		private string _text;
		private string _specificationName;

		protected override void Context()
		{
			_text = "ends_with_underscore_";
		}

		protected override void Because()
		{
			_specificationName = SpecificationName.GetName(_text);
		}

		[Observation]
		public void should_replace_underscores_with_spaces()
		{
			_specificationName.ShouldEqual("ends with underscore");
		}
	}

	[Concern(typeof(SpecificationName))]
	public class when_the_name_has_two_consecutive_underscores : ContextSpecification
	{
		private string _text;
		private string _specificationName;

		protected override void Context()
		{
			_text = "includes__quoted__text_and__more__text";
		}

		protected override void Because()
		{
			_specificationName = SpecificationName.GetName(_text);
		}

		[Observation]
		public void should_replace_the_underscores_with_a_quotation_mark()
		{
			_specificationName.ShouldEqual("includes \"quoted\" text and \"more\" text");
		}
	}

	[Concern(typeof(SpecificationName))]
	public class when_the_name_has_an_underscore_followed_by_an__S__followed_by_an_underscore : ContextSpecification
	{
		private string _text;
		private string _specificationName;

		protected override void Context()
		{
			_text = "it_s_possessive";
		}

		protected override void Because()
		{
			_specificationName = SpecificationName.GetName(_text);
		}

		[Observation]
		public void should_replace_it_with_appostrophy__S__()
		{
			_specificationName.ShouldEqual("it's possessive");
		}
	}
}