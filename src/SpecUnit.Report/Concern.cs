using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecUnit.Report
{
	public class Concern
	{
		private string _name;
		private readonly List<Context> _contexts = new List<Context>();

		public string Name
		{
			get { return _name; }
		}

		public Context[] Contexts
		{
			get { return _contexts.ToArray(); }
		}

		public Concern(string name)
		{
			_name = name;
		}

		public Concern(Type concernType) : this(concernType.Name) {}

		public void AddContextFor(Type testFixtureType)
		{
			string concernName = testFixtureType.GetConcernName();

			if (concernName != _name && _name != null)
			{
				throw new Exception(String.Format("{0} does not belong to the {1} concern.", testFixtureType.Name, _name));
			}

			Context context = Context.Build(testFixtureType);
			_contexts.Add(context);
		}

		public bool HasContextFor(Type type)
		{
			return (_contexts.Count(c => c.TestFixtureType == type)) != 0;
		}
	}

	public static class ConcernExtentions
	{
		public static bool HasConcern(this Type type)
		{
			return type.GetCustomAttributes(typeof(ConcernAttribute), true).Length != 0;
		}

		public static string GetConcernName(this Type type)
		{
			object[] attributes = type.GetCustomAttributes(typeof (ConcernAttribute), true);

			if (attributes.Length == 0)
			{
				return null;
			}

			ConcernAttribute concernAttribute = (ConcernAttribute) attributes[0];

			return concernAttribute.Name;
		}
	}
}