using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class ProstorZaProbuMockRepository
    {
        public List<ProstorZaProbu> GetAll()
        {
            return MockDataStore.Prostori;
        }

        public ProstorZaProbu? GetById(int id)
        {
            return MockDataStore.Prostori.FirstOrDefault(p => p.Id == id);
        }
    }
}