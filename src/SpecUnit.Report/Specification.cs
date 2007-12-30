using System.Reflection;
using NUnit.Framework;

namespace SpecUnit.Report
{
	public class Specification : SpecificationName
	{
		private readonly string _testCaseName;

		public string TestCaseName
		{
			get { return _testCaseName; }
		}

		public string Name
		{
			get { return GetName(); }
		}

		public Specification(string testCaseName)
		{
			_testCaseName = testCaseName;
		}

		private string GetName()
		{
			string name = GetName(_testCaseName);

			if (name.IndexOf(" when ") != -1 || name.IndexOf(" if ") != -1)
			{
				name += " -- WARNING: Specifications with the words \"if\" or \"when\" may be contexts";
			}

			return name;
		}

		public static bool IsTestMethod(MethodInfo method)
		{
//			return method.GetCustomAttributes(typeof(TestAttribute), false).Length != 0;
			return method.IsTestMethod();
		}
	}

	public static class SpecificationExtensions
	{
		public static bool IsTestMethod(this MethodInfo method)
		{
			return method.GetCustomAttributes(typeof(TestAttribute), false).Length != 0;
		}
	}
}