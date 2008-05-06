using System.Reflection;
using NUnit.Framework;

namespace SpecUnit.Report
{
	public class Specification : SpecificationName
	{
		private static readonly bool defaultWarnOfContextNaming = true;

		private readonly string _testCaseName;
		private readonly bool _warnOfContextNaming = defaultWarnOfContextNaming;

		public string TestCaseName
		{
			get { return _testCaseName; }
		}

		public string Name
		{
			get { return GetName(); }
		}

		public Specification(string testCaseName) : this (testCaseName, defaultWarnOfContextNaming) {}

		public Specification(string testCaseName, bool warnOfContextNaming)
		{
			_testCaseName = testCaseName;
			_warnOfContextNaming = warnOfContextNaming;
		}

		private string GetName()
		{
			string name = GetName(_testCaseName);

			if (_warnOfContextNaming == true && (name.IndexOf(" when ") != -1 || name.IndexOf(" if ") != -1))
			{
				name += " -- WARNING: Specifications with the words \"if\" or \"when\" may be contexts";
			}

			return name;
		}

		public static bool IsTestMethod(MethodInfo method)
		{
			return method.IsTestMethod();
		}
	}

	public static class SpecificationExtensions
	{
		public static bool IsTestMethod(this MethodInfo method)
		{
			return method.GetCustomAttributes(typeof(SpecificationAttribute), false).Length != 0;
		}
	}
}