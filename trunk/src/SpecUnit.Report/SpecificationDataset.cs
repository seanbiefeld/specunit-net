using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;
using System.Linq;

namespace SpecUnit.Report
{
	public class SpecificationDataset
	{
		private readonly Assembly _assembly;
		private List<Concern> _concerns = new List<Concern>();

		public Assembly Assembly
		{
			get { return _assembly; }
		}

		public Concern[] Concerns
		{
			get { return _concerns.ToArray(); }
		}

		public string Name
		{
			get { return GetName(_assembly.GetName().Name); }
		}

		public SpecificationDataset(Assembly assembly)
		{
			_assembly = assembly;
		}

		private string GetName(string assemblyName)
		{
			int endPosition = assemblyName.LastIndexOf(".");

			return assemblyName.Substring(0, endPosition);
		}

		public void BuildConcerns()
		{
			Type[] testFixtureTypes = _assembly.GetTypes().GetConcreteTestFixtureTypes();

			foreach (Type testFixtureType in testFixtureTypes)
			{
				if (testFixtureType.HasConcern())
				{
					Concern concern =
						(from c in _concerns
						where c.Name == testFixtureType.GetConcernName()
						select c).FirstOrDefault();

					if (concern == null)
					{
						concern = new Concern(testFixtureType.GetConcernName());
						_concerns.Add(concern);
					}

					concern.AddContextFor(testFixtureType);
				}
			}
		}

		public static Type[] GetConcreteTestFixtureTypes(Type[] types)
		{
			List<Type> testFixtureTypes = new List<Type>();

			Assembly baseAssembly = types[0].Assembly;

			foreach (Type type in types)
			{
				if (type.IsAbstract == false && TypeHasTestFixtureAttribute(type))
				{
					testFixtureTypes.Add(type);
				}
			}

			return testFixtureTypes.ToArray();
		}

		private static bool TypeHasTestFixtureAttribute(Type type)
		{
			bool look_for_attribute_on_base_types = true;
			return type.GetCustomAttributes(typeof(TestFixtureAttribute), look_for_attribute_on_base_types).Length != 0;
		}

		public static SpecificationDataset Build(Assembly assemblyUnderTest)
		{
			SpecificationDataset specificationDataset = new SpecificationDataset(assemblyUnderTest);
			specificationDataset.BuildConcerns();
			return specificationDataset;
		}
	}

	public static class SpecificationDatasetExtensions
	{
		public static Type[] GetConcreteTestFixtureTypes(this Type[] types)
		{
			List<Type> testFixtureTypes = new List<Type>();

			Assembly baseAssembly = types[0].Assembly;

			foreach (Type type in types)
			{
				if (type.IsAbstract == false && TypeHasTestFixtureAttribute(type))
				{
					testFixtureTypes.Add(type);
				}
			}

			return testFixtureTypes.ToArray();
		}

		private static bool TypeHasTestFixtureAttribute(Type type)
		{
			bool look_for_attribute_on_base_types = true;
			return type.GetCustomAttributes(typeof(TestFixtureAttribute), look_for_attribute_on_base_types).Length != 0;
		}
	}
}