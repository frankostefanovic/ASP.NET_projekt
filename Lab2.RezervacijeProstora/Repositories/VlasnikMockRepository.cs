using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class VlasnikMockRepository
    {
        public List<Vlasnik> GetAll()
        {
            return MockDataStore.Vlasnici;
        }

        public Vlasnik? GetById(int id)
        {
            return MockDataStore.Vlasnici.FirstOrDefault(v => v.Id == id);
        }
    }
}