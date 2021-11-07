namespace ELDEL_EntityFramework_Library.Repositories
{
    public class ChargerDetailsRepository : IChargerDetailsRepository
    {
        private readonly EldelContext _context;

        public ChargerDetailsRepository(EldelContext context)
        {
            _context = context;
        }
    }
}
