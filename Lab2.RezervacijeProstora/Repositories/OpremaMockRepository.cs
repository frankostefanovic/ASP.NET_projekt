using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class OpremaMockRepository
    {
        public List<Oprema> GetAll()
        {
            return MockDataStore.Oprema;
        }

        public Oprema? GetById(int id)
        {
            return MockDataStore.Oprema.FirstOrDefault(o => o.Id == id);
        }
    }
}