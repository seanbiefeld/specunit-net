using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NUnit.Framework;

namespace SpecUnit.Report
{
	public class Context : SpecificationName, IEquatable<Context>
	{
		private readonly Type _testFixtureType;
		private readonly List<Specification> _specifications = new List<Specification>();

		public Type TestFixtureType
		{
			get { return _testFixtureType; }
		}

		public string Name
		{
			get { return GetName(); }
		}

		public string ConcernName
		{
			get { return _testFixtureType.GetConcernName(); }
		}

		public Specification[] Specifications
		{
			get { return _specifications.ToArray(); }
		}

		public string BehavesLike
		{
			get { return _testFixtureType.BehavesLike(); }
		}

		public Context(Type type)
		{
			AssertIsTestFixture(type);

			_testFixtureType = type;
		}

		private void AssertIsTestFixture(Type type)
		{
			if (type.IsTestFixture() == false)
			{
				throw new ArgumentException(String.Format("{0} is not a test fixture.", type.Name));
			}
		}

		public static Context Build(Type testFixtureType)
		{
			Context context = new Context(testFixtureType);

			context.BuildSpecifications();

			return context;
		}

		private void BuildSpecifications()
		{
			MethodInfo[] methods = _testFixtureType.GetMethods();

			foreach (MethodInfo method in methods)
			{
				if (method.IsTestMethod())
				{
					AddSpecificationFor(method);
				}
			}
		}

		private string GetName()
		{
			string name = _testFixtureType.Name;

			name = GetName(name);

			string concernName = _testFixtureType.GetConcernName();
			if (concernName != null)
			{
				name = concernName + ", " + name;
			}

			return name;
		}

		public static bool HasConcern(Type type)
		{
			return type.HasConcern();
		}

		private void AddSpecificationFor(MethodInfo method)
		{
			Specification specification = new Specification(method.Name);
			_specifications.Add(specification);
		}

		public override bool Equals(object obj)
		{
			return ((IEquatable<Context>)this).Equals((Context)obj);
		}

		public bool Equals(Context other)
		{
			return _testFixtureType == other.TestFixtureType;
		}

		public bool HasSpecificationFor(string testCaseName)
		{
			return _specifications.Count(s => s.TestCaseName == testCaseName) != 0;
		}
	}

	public static class ContextEntensions
	{
		public static bool IsTestFixture(this Type type)
		{
			bool do_not_look_for_attribute_on_base_types = true;
			return type.GetCustomAttributes(typeof(TestFixtureAttribute), do_not_look_for_attribute_on_base_types).Length != 0;
		}

		public static string BehavesLike(this Type type)
		{
			Type baseType = type.BaseType;

			if (baseType == null)
			{
				return null;
			}

			if (baseType.Name.ToLowerInvariant().StartsWith("behaves_like") == false)
			{
				return null;
			}

			string name = baseType.Name.Remove(0, "behaves_like".Length);

			return SpecificationName.GetName(name);
		}
	}
}