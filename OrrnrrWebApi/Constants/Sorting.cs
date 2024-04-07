
namespace OrrnrrWebApi.Constants
{
    public static class Sorting
    {
        public const string ASC = "ASC";
        public const string DESC = "DESC";

        internal static bool Contains(string sorting)
        {
            return sorting == ASC || sorting == DESC;
        }
    }
}
