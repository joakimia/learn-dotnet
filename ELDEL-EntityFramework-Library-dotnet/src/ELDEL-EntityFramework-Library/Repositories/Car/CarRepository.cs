namespace ELDEL_EntityFramework_Library.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly EldelContext _context;

        public CarRepository(EldelContext context)
        {
            _context = context;
        }
    }
}
