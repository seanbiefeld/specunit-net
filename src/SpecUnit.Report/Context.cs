using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace SpecUnit.Report
{
	public class Context : SpecificationName, IEquatable<Context>
	{
		private readonly Type _testFixtureType;

		public Type TestFixtureType
		{
			get { return _testFixtureType; }
		}

		public string Name
		{
			get
			{
				return GetName();
			}
		}

		public string ConcernName
		{
			get
			{
				return GetConcernName();
			}
		}

		public Context(Type testFixtureType)
		{
			if (IsTestFixture(testFixtureType) == false)
			{
				throw new ArgumentException(String.Format("{0} is not a test fixture.", testFixtureType.Name));
			}

			_testFixtureType = testFixtureType;
		}

		private string GetName()
		{
			string name = _testFixtureType.Name;

			name = GetName(name);

			string concernName = GetConcernName();
			if (concernName != null)
			{
				name = concernName + ", " + name;
			}

			return name;
		}

		private string GetConcernName()
		{
			bool do_not_look_for_attribute_on_base_types = true;

			object[] attributes = _testFixtureType.GetCustomAttributes(typeof(ConcernAttribute), do_not_look_for_attribute_on_base_types);

			if (attributes.Length == 0)
			{
				return null;
			}

			ConcernAttribute concernAttribute = (ConcernAttribute)attributes[0];

			return concernAttribute.Name;
		}

		public static bool IsTestFixture(Type type)
		{
			bool do_not_look_for_attribute_on_base_types = true;
			return type.GetCustomAttributes(typeof(TestFixtureAttribute), do_not_look_for_attribute_on_base_types).Length != 0;
		}

		public static bool HasConcern(Type type)
		{
			bool look_for_attribute_on_base_types = false;
			return type.GetCustomAttributes(typeof(ConcernAttribute), look_for_attribute_on_base_types).Length != 0;
		}

		public Specification[] GetSpecifications()
		{
			List<Specification> specifications = new List<Specification>();

			MethodInfo[] methods = _testFixtureType.GetMethods();

			foreach (MethodInfo method in methods)
			{
				if (Specification.IsTestMethod(method))
				{
					Specification specification = new Specification(method.Name);
					specifications.Add(specification);
				}
			}

			return specifications.ToArray();
		}

		public override bool Equals(object obj)
		{
			return ((IEquatable<Context>)this).Equals((Context)obj);
		}

		public bool Equals(Context other)
		{
			return this._testFixtureType == other.TestFixtureType;
		}
	}
}