using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace SpecUnit.Report
{
	public class ReportGenerator
	{
		public static string Render(SpecificationDataset specificationDataset)
		{
			StringBuilder reportBuilder = new StringBuilder();

			RenderTitle(specificationDataset, reportBuilder);

			Concern[] concerns =
				(from c in specificationDataset.Concerns
				orderby c.Name
				select c).ToArray();

			RenderConcerns(concerns, reportBuilder);

			string reportBody = reportBuilder.ToString();

			return String.Format(GetTemplate(), specificationDataset.GetName(), reportBody);
		}

		private static void RenderTitle(SpecificationDataset specificationDataset, StringBuilder reportBuilder)
		{
			int contextCount = specificationDataset.Concerns.Sum(c => c.Contexts.Length);
			int specificationCount = specificationDataset.Concerns.Sum(c => c.Contexts.Sum(ctx => ctx.Specifications.Length));

			string title = String.Format("<h1>{0}&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"count\">{1} concern(s), {2} context(s), {3} specification(s)</span></h1>\n\n", specificationDataset.GetName(), specificationDataset.Concerns.Length, contextCount, specificationCount);
			reportBuilder.Append(title);
			RenderHR(reportBuilder);
		}

		private static void RenderConcerns(Concern[] concerns, StringBuilder reportBuilder)
		{
			foreach (Concern concern in concerns)
			{
				RenderConcern(concern, reportBuilder);
			}
		}

		private static void RenderConcern(Concern concern, StringBuilder reportBuilder)
		{
			string concernText = RenderConcern(concern);
			reportBuilder.Append(concernText);
		}

		public static string RenderConcern(Concern concern)
		{
			StringBuilder reportBuilder = new StringBuilder();

			string concernHeader = RenderConcernHeader(concern);
			concernHeader = String.Format("{0}\n\n", concernHeader);
			reportBuilder.Append(concernHeader);

			RenderContexts(concern.Contexts, reportBuilder);

			RenderHR(reportBuilder);

			return reportBuilder.ToString();
		}

		private static void RenderHR(StringBuilder reportBuilder)
		{
			string hr = "<hr>\n\n";
			reportBuilder.Append(hr);
		}

		public static string RenderConcernHeader(Concern concern)
		{
			int specificationCount = concern.Contexts.Sum(c => c.Specifications.Length);

			return String.Format("<h2 class=\"concern\">{0} specifications&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"count\">{1} context(s), {2} specification(s)</span></h2>", concern.Name, concern.Contexts.Length, specificationCount);
		}

		private static void RenderContexts(Context[] contexts, StringBuilder reportBuilder)
		{
			foreach (Context context in contexts)
			{
				string specificationClassHeader = RenderContextHeader(context);
				specificationClassHeader = String.Format("{0}\n", specificationClassHeader);
				reportBuilder.Append(specificationClassHeader);

				string specificationList = RenderSpecificationList(context.Specifications);
				reportBuilder.Append(specificationList);
			}
		}

		public static string RenderContextHeader(Context context)
		{
			return String.Format("<h3 class=\"context\">{0}&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"count\">{1} specification(s)</span></h3>", context.Name, context.Specifications.Length);
		}

		public static string RenderSpecificationList(Specification[] specifications)
		{
			StringBuilder specificationListBuilder = new StringBuilder();

			foreach (Specification specification in specifications)
			{
				string specificationListItem = String.Format("\t<li>{0}</li>\n", specification.Name);
				specificationListBuilder.Append(specificationListItem);
			}

			return String.Format("<ul>\n{0}</ul>\n\n", specificationListBuilder);
		}

		private static string GetTemplate()
		{
			string template = @"<html>
	<head>
		<title>Specification Report for {0}</title>
		<style type=""text/css"">
			body {{
				font-family: Arial,Helvetica,sans-serif;
				font-size: .9em;
			}}

			.count {{
				color: LightGrey;
			}}

			hr {{
				color: LightGrey;
				border: 1px solid LightGrey;
				height: 1px;
			}}
		</style>
	</head>
	<body>
		{1}
	</body>
</html>";
			return template;
		}

		public virtual void WriteReport(Assembly assemblyUnderTest)
		{
			SpecificationDataset specificationDataset = SpecificationDataset.Build(assemblyUnderTest);

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