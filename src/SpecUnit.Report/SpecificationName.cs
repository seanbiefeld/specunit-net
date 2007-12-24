using System.Text.RegularExpressions;

namespace SpecUnit.Report
{
    public abstract class SpecificationName
    {
        public static string GetName(string text)
        {
            text = ReplaceDoubleUnderscoresWithQuotes(text);

            text = ReplaceUnderscoreEssWithPossessive(text);

            text = ReplaceSingleUnderscoresWithSpaces(text);

            text = text.Trim();

            return text;
        }

        private static string ReplaceUnderscoreEssWithPossessive(string specificationName)
        {
            specificationName = specificationName.Replace("_s_", "'s ");
            return specificationName;
        }

        private static string ReplaceSingleUnderscoresWithSpaces(string specificationName)
        {
            specificationName = specificationName.Replace("_", " ");
            return specificationName;
        }

        private static string ReplaceDoubleUnderscoresWithQuotes(string specificationName)
        {
            Regex regex = new Regex(@"(?<quoted>__(?<inner>\w+?)__)");

            specificationName = Regex.Replace(specificationName, @"(?<quoted>__(?<inner>\w+?)__)", " \"${inner}\" ");

            return specificationName;
        }
    }
}