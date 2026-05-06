using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class PlacanjeMockRepository
    {
        public List<Placanje> GetAll()
        {
            return MockDataStore.Placanja;
        }

        public Placanje? GetById(int id)
        {
            return MockDataStore.Placanja.FirstOrDefault(p => p.Id == id);
        }
    }
}