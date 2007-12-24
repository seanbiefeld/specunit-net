using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[Concern(typeof(ReportGenerator))]
	public class when_rendering_a_context : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = new Context(typeof(TestFixture));
		}

		[Observation]
		public void should_render_the_name_as_an_H4()
		{
			ReportGenerator.RenderSpecificationClassHeader(_context).ShouldEqual("<h4>TestFixture</h4>");
		}

		[Observation]
		public void should_render_specifications_as_a_bulletted_list()
		{
			string expectedText = "<ul>\n\t<li>TestCase1</li>\n\t<li>TestCase2</li>\n</ul>";

			ReportGenerator.RenderSpecificationList(_context.GetSpecifications()).ShouldEqual(expectedText);
		}
	}

	[Concern(typeof(ReportGenerator))]
	public class when_writing_the_spec_report_for_an_assembly : ContextSpecification
	{
		private ReportGenerator _reportGenerator;
		private Assembly _assembly;
		private string _assemblyFilePath;

		protected override void Context()
		{
			_reportGenerator = new ReportGenerator();

			_assembly = Assembly.GetExecutingAssembly();
			_assemblyFilePath = _assembly.GetName().Name + ".dll";
		}

		protected override void Because()
		{
			_reportGenerator.WriteReport(_assembly);
		}

		[Observation]
		public void should_write_an_html_file_named_after_the_assembly_that_the_report_is_created_for()
		{
			File.Exists(_assemblyFilePath).ShouldBeTrue();
		}
	}
}