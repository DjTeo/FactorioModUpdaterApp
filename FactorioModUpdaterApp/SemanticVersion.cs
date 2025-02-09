using System.Text.RegularExpressions;

namespace FactorioModUpdaterApp
{
    public class SemanticVersion : IComparable<SemanticVersion>
    {
        public SemanticVersion()
        {
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }

        public static SemanticVersion Parse(string input)
        {
            Match match = Regex
                .Match(input, @"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)$");
            if (match.Success)
            {
                return new SemanticVersion()
                {
                    Major = int.Parse(match.Groups["major"].Value),
                    Minor = int.Parse(match.Groups["minor"].Value),
                    Patch = int.Parse(match.Groups["patch"].Value),
                };
            }
            return new SemanticVersion();
        }

        public int CompareTo(SemanticVersion other)
        {
            int result = Major.CompareTo(other.Major);
            if (result == 0)
            {
                result = Minor.CompareTo(other.Minor);
            }
            if (result == 0)
            {
                result = Patch.CompareTo(other.Patch);
            }
            return result;
        }
    }
}
