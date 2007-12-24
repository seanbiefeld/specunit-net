using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SpecUnit.Report
{
	public class ReportGenerator
	{
		public static string RenderSpecificationClassHeader(Context context)
		{
			return String.Format("<h4>{0}</h4>", context.Name);
		}

		public static string RenderSpecificationList(Specification[] specifications)
		{
			StringBuilder specificationListBuilder = new StringBuilder();

			foreach (Specification specification in specifications)
			{
				string specificationListItem = String.Format("\t<li>{0}</li>\n", specification.Name);
				specificationListBuilder.Append(specificationListItem);
			}

			return String.Format("<ul>\n{0}</ul>", specificationListBuilder);
		}

		public static string Render(SpecificationDataset specificationDataset)
		{
			StringBuilder reportBuilder = new StringBuilder();

			string title = String.Format("<h3>{0}</h3>\n", specificationDataset.GetName());
			reportBuilder.Append(title);

			specificationDataset.BuildSpecificationClasses();

			foreach (Context specificationClass in specificationDataset.Contexts)
			{
				string specificationClassHeader = RenderSpecificationClassHeader(specificationClass);
				specificationClassHeader = String.Format("{0}\n", specificationClassHeader);
				reportBuilder.Append(specificationClassHeader);

				string specificationList = RenderSpecificationList(specificationClass.GetSpecifications());
				reportBuilder.Append(specificationList);
			}

			string reportBody = reportBuilder.ToString();

			return String.Format(GetTemplate(), specificationDataset.GetName(), reportBody);
		}

		private static string GetTemplate()
		{
			string template = @"<html>
	<head>
		<title>Specification Report for {0}</title>
	<body>
		{1}
	</body>
</html>";
			return template;
		}

		public virtual void WriteReport(Assembly assemblyUnderTest)
		{
			SpecificationDataset specificationDataset = new SpecificationDataset(assemblyUnderTest);

			string generatedReport = Render(specificationDataset);

			string reportFilePath = assemblyUnderTest.GetName().Name + ".html";

			if (File.Exists(reportFilePath))
			{
				File.Delete(reportFilePath);
			}

			TextWriter tw = new StreamWriter(reportFilePath);

			tw.Write(generatedReport);
			tw.Close();
		}
	}
}