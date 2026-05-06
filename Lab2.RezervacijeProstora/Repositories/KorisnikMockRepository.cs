using Lab2.RezervacijeProstora.Models;
using System.Linq;

namespace Lab2.RezervacijeProstora.Repositories
{
    public class KorisnikMockRepository
    {
        public List<Korisnik> GetAll()
        {
            return MockDataStore.Korisnici;
        }

        public Korisnik? GetById(int id)
        {
            return MockDataStore.Korisnici.FirstOrDefault(k => k.Id == id);
        }
    }
}