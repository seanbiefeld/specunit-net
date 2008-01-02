using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[TestFixture]
	[Concern(typeof(SpecificationDataset))]
	public class when_building_a_dataset_for_an_assembly : ContextSpecification
	{
		private SpecificationDataset _specificationDataset;
		private Assembly _assemblyUnderTest;

		protected override void Context()
		{
			_assemblyUnderTest = typeof(A_fixture).Assembly;
		}

		protected override void Because()
		{
			_specificationDataset = SpecificationDataset.Build(_assemblyUnderTest);
		}

		[Test]
		[Observation]
		public void should_collect_and_build_the_assembly_s_concerns()
		{
			_specificationDataset.Concerns.Length.ShouldBeGreaterThan(0);
		}

		[Test]
		[Observation]
		public void should_be_named_for_the_file_stem_of_the_of_the_assembly_name()
		{
			_specificationDataset.Name.ShouldEqual("SpecUnit.Specs");
		}
	}

	[Concern(typeof(SpecificationDataset))]
	public class when_collecting_concerns_from_an_assembly : ContextSpecification
	{
		private SpecificationDataset _specificationDataset;

		protected override void Context()
		{
			Assembly assemblyUnderTest = typeof(Context_with_concern).Assembly;
			_specificationDataset = new SpecificationDataset(assemblyUnderTest);
		}

		protected override void Because( /* there are three classes in the assembly with two unique concerns */ )
		{
			_specificationDataset.BuildConcerns();
		}

		[Observation]
		public void should_include_one_concern_for_each_unique_concern_found()
		{
			_specificationDataset.Concerns.Length.ShouldEqual(2);
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
		public void should_include_concrete_classes_with_the_TestFixture_attribute()
		{
			_datasetTypes[0].ShouldEqual(typeof(A_fixture));
		}
	}
}