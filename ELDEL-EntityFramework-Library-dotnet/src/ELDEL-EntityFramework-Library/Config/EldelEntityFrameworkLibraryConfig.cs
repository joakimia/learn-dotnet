namespace ELDEL_EntityFramework_Library.Config
{
    public interface IEldelEntityFrameworkLibraryConfig
    {
        string ConnectionString { get; }
    }

    public class EldelEntityFrameworkLibraryConfig : IEldelEntityFrameworkLibraryConfig
    {
        public string ConnectionString { get; set; }
    }
}
