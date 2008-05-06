using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;
using System.Linq;

namespace SpecUnit.Specs
{
	public abstract class behaves_like_dataset_with_contexts : ContextSpecification
	{
		protected SpecificationDataset _specificationDataset;
		protected Assembly _assembly;

		protected override void SharedContext()
		{
			_assembly = typeof(A_fixture).Assembly;
		}

		protected override void Context()
		{
			_specificationDataset = SpecificationDataset.Build(_assembly);
		}
	}

	[TestFixture]
	[Concern(typeof(SpecificationDataset))]
	public class when_building_a_dataset_for_an_assembly
		: behaves_like_dataset_with_contexts
	{
		[Test]
		[Observation]
		public void should_collect_and_build_the_assembly_s_concerns()
		{
			_specificationDataset.Concerns.Length.ShouldBeGreaterThan(0);
		}
	}

	[TestFixture]
	[Concern(typeof(SpecificationDataset))]
	public class when_naming_a_specification_dataset_for_an_assembly_that_has_a_period_in_its_name
		: behaves_like_dataset_with_contexts
	{
		[Test]
		[Observation]
		public void should_be_named_for_the_part_of_stem_of_the_assembly_filename_that_comes_before_the_last_period()
		{
			_specificationDataset.Name.ShouldEqual("SpecUnit.Specs");
		}
	}

	[TestFixture]
	[Concern(typeof(SpecificationDataset))]
	public class when_naming_a_specification_dataset_for_an_assembly_that_does_not_have_a_period_in_its_name : ContextSpecification
	{
		protected SpecificationDataset _specificationDataset;
		protected Assembly _assembly;

		protected override void Context()
		{
			_assembly = typeof(ClassFromAssemblyWithoutPeriodInName).Assembly;
			_specificationDataset = SpecificationDataset.Build(_assembly);
		}

		[Test]
		[Observation]
		public void should_be_named_for_the_stem_of_the_assembly_filename()
		{
			_specificationDataset.Name.ShouldEqual("AssemblyWithoutPeriodInName");
		}
	}

	[TestFixture]
	[Concern(typeof(SpecificationDataset))]
	public class when_collecting_concerns_from_an_assembly
		: behaves_like_dataset_with_contexts
	{
		protected override void Context()
		{
			_specificationDataset = new SpecificationDataset(_assembly);
		}

		protected override void Because(/* there are three classes in the assembly with two unique concerns */)
		{
			_specificationDataset.BuildConcerns();
		}

		[Test]
		[Observation]
		public void should_include_one_concern_for_each_unique_concern_found()
		{
			_specificationDataset.Concerns.Length.ShouldEqual(2);
		}
	}

	[TestFixture]
	[Concern(typeof(SpecificationDataset), "collecting concerns")]
	public class when_filtering_based_on_a_concern_name
		: behaves_like_dataset_with_contexts
	{
		protected override void Context()
		{
			_specificationDataset = new SpecificationDataset(_assembly);
		}

		protected override void Because(/* there are three classes in the assembly with two unique concerns */)
		{
			_specificationDataset.BuildConcerns("SomeConcern");
		}

		[Test]
		[Observation]
		public void should_include_contexts_whose_conerns_not_match_the_filter()
		{
			_specificationDataset.Concerns[0].Name.ShouldEqual("SomeConcern");
		}

		[Test]
		[Observation]
		public void should_exclude_contexts_whose_conerns_do_not_match_the_filter()
		{
			var concerns = _specificationDataset.Concerns.Where(c => c.Name == "SomeOtherConcern");

			Concern concern = concerns.FirstOrDefault();

			concern.ShouldBeNull();
		}
	}

	[Concern(typeof(SpecificationDataset))]
	public class when_selecting_context_classes_from_a_list_of_types : ContextSpecification
	{
		private Type[] _sourceTypes;
		private Type[] _datasetTypes;
		private Type _concreteFixtureType;
		private Type _abstractFixtureType;

		protected override void Context()
		{
			List<Type> types = new List<Type>();

			_abstractFixtureType = typeof(AbstractFixture);
			_concreteFixtureType = typeof(A_fixture);

			types.Add(_concreteFixtureType);
			types.Add(_abstractFixtureType);
			types.Add(typeof(SomeConcern));

			_sourceTypes = types.ToArray();
		}

		protected override void Because(/* not all types are test fixtures */)
		{
			_datasetTypes = _sourceTypes.GetConcreteTestFixtureTypes();
		}

		[Observation]
		public void should_ignore_abstract_types()
		{
			_datasetTypes.ShouldNotContain(_abstractFixtureType);
		}

		[Observation]
		public void should_include_concrete_classes_attributed_with_an_attribute_in_the_TestFixtureAttribute_family()
		{
			_datasetTypes[0].ShouldEqual(typeof(A_fixture));
		}
	}
}