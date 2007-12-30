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
		private Context[] _contexts = new Context[0];
		private List<Concern> _concerns = new List<Concern>();

		public Assembly Assembly
		{
			get { return _assembly; }
		}

		public Context[] Contexts
		{
			get { return _contexts; }
		}

		public Concern[] Concerns
		{
			get { return _concerns.ToArray(); }
		}

		public SpecificationDataset(Assembly assembly)
		{
			_assembly = assembly;
		}

		public string GetName()
		{
			string assemblyName = _assembly.GetName().Name;

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

		public void BuildContexts()
		{
			Type[] testFixtureTypes = _assembly.GetTypes().GetConcreteTestFixtureTypes();

			List<Context> contexts = new List<Context>();
			foreach (Type testFixtureType in testFixtureTypes)
			{
				Context context = new Context(testFixtureType);
				contexts.Add(context);
			}

			_contexts =
				(from c in contexts
				 orderby c.Name
				 select c).ToArray();
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