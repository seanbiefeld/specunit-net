using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SpecUnit.Report;
using SpecUnit.Specs.AssemblyUnderTest;

namespace SpecUnit.Specs
{
	[TestFixture]
	[Concern(typeof(ReportGenerator))]
	public class when_rendering_a_concern : ContextSpecification
	{
		private Concern _concern;

		protected override void Context()
		{
			_concern = new Concern(typeof(SomeConcern));
			_concern.AddContextFor(typeof(Context_with_concern));
			_concern.AddContextFor(typeof(Context_with_same_concern));
		}

		[Test]
		[Observation]
		public void should_render_the_name_in_an_H2()
		{
			ReportGenerator.RenderConcernHeader(_concern).ShouldBeSurroundedWith("<h2", "</h2>");
		}

		[Test]
		[Observation]
		public void should_render_the_count_of_contexts_and_specifications()
		{
			ReportGenerator.RenderConcernHeader(_concern).ShouldContain("[2 context(s), 4 specification(s)]");
		}

		[Test]
		[Observation]
		public void should_render_its_contexts()
		{
			ReportGenerator.RenderConcern(_concern).ShouldContain("Context with concern");
		}
	}

	[TestFixture]
	[Concern(typeof(ReportGenerator))]
	public class when_rendering_the_context_for_a_concern : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = SpecUnit.Report.Context.Build(typeof(TestFixture));
		}

		[Observation]
		public void should_render_the_name_in_an_H3()
		{
			ReportGenerator.RenderContextHeader(_context).ShouldBeSurroundedWith("<h3", "</h3>");
		}

		[Test]
		[Observation]
		public void should_render_the_count_of_specifications_in_the_header()
		{
			ReportGenerator.RenderContextHeader(_context).ShouldContain("[2 specification(s)]");
		}

		[Observation]
		public void should_render_specifications_as_a_bulletted_list()
		{
			string expectedText = "<ul>\n\t<li>TestCase1</li>\n\t<li>TestCase2</li>\n</ul>";

			ReportGenerator.RenderSpecificationList(_context.Specifications).ShouldContain(expectedText);
		}
	}

	[TestFixture]
	[Concern(typeof(ReportGenerator))]
	public class when_rendering_a_context : ContextSpecification
	{
		private Context _context;

		protected override void Context()
		{
			_context = SpecUnit.Report.Context.Build(typeof(TestFixture));
		}

		[Observation]
		public void should_render_the_name_in_an_H3()
		{
			ReportGenerator.RenderContextHeader(_context).ShouldBeSurroundedWith("<h3", "</h3>");
		}

		[Test]
		[Observation]
		public void should_render_the_count_of_specifications_in_the_header()
		{
			ReportGenerator.RenderContextHeader(_context).ShouldContain("[2 specification(s)]");
		}

		[Observation]
		public void should_render_specifications_as_a_bulletted_list()
		{
			string expectedText = "<ul>\n\t<li>TestCase1</li>\n\t<li>TestCase2</li>\n</ul>";

			ReportGenerator.RenderSpecificationList(_context.Specifications).ShouldContain(expectedText);
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