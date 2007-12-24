using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[Concern(typeof(SpecificationDataset))]
	public class when_creating_a_dataset_for_an_assembly : ContextSpecification
	{
		private SpecificationDataset _specificationDataset;
		private Assembly _assemblyUnderTest;

		protected override void Context()
		{
			_assemblyUnderTest = typeof(A_fixture).Assembly;
		}

		protected override void Because(/* the assembly under test is named SpecUnit.Specs.AssemblyUnderTest */)
		{
			_specificationDataset = new SpecificationDataset(_assemblyUnderTest);
		}

		[Observation]
		public void should_be_named_for_the_file_stem_of_the_name_of_the_assembly()
		{
			_specificationDataset.GetName().ShouldEqual("SpecUnit.Specs");
		}
	}

	[Concern(typeof(SpecificationDataset))]
	public class when_selecting_context_classes_from_an_assembly : ContextSpecification
	{
		private SpecificationDataset _specificationDataset;

		protected override void Context()
		{
			Assembly assemblyUnderTest = typeof(A_fixture).Assembly;
			_specificationDataset = new SpecificationDataset(assemblyUnderTest);
		}

		protected override void Because(/* SpecUnit.Specs.AssemblyUnderTest has concrete test classes */)
		{
			_specificationDataset.BuildSpecificationClasses();
		}

		[Observation]
		public void should_include_concrete_classes_with_the_TestFixture_attribute()
		{
			_specificationDataset.Contexts.Length.ShouldEqual(7);
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

		protected override void Because(/* not all types are selected */)
		{
			_datasetTypes = SpecificationDataset.GetConcreteTestFixtureTypes(_sourceTypes);
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

	[Concern(typeof(SpecificationDataset))]
	public class when_selecting_context_classes_from_a_list_of_types_where_the_types_are_not_from_the_same_assembly : ContextSpecification
	{
		private List<Type> types;

		protected override void Context()
		{
			types = new List<Type>();
			types.Add(typeof(A_fixture));
		}

		protected override void Because(/* System.Object is not a member of the same assembly as A_fixture */)
		{
			types.Add(typeof(Object));
		}

		[Observation]
		public void should_fail()
		{
			typeof(ArgumentException).ShouldBeThrownBy(delegate
				{
					SpecificationDataset.GetConcreteTestFixtureTypes(types.ToArray());
				});
		}
	}

	[Concern(typeof(SpecificationDataset))]
	[TestFixture]
	public class when_sorting_contexts_by_context_name : ContextSpecification
	{
		private SpecificationDataset _SpecificationDataset;

		protected override void Context()
		{
			Assembly assemblyUnderTest = Assembly.Load("SpecUnit.Specs.AssemblyUnderTest");
			_SpecificationDataset = new SpecificationDataset(assemblyUnderTest);
		}

		protected override void Because()
		{
			_SpecificationDataset.BuildSpecificationClasses();
		}

		[Observation]
		public void should_place_the_first_context_at_the_beginning_of_the_list_of_contexts()
		{
			_SpecificationDataset.Contexts[0].Name.ShouldBeLessThan(_SpecificationDataset.Contexts[1].Name);
		}

		[Observation]
		public void should_place_the_last_context_at_the_end_of_the_list_of_contexts()
		{
			_SpecificationDataset.Contexts[_SpecificationDataset.Contexts.Length - 1].Name.ShouldBeGreaterThan(_SpecificationDataset.Contexts[_SpecificationDataset.Contexts.Length - 2].Name);
		}
	}
}