using Shop.Models;

namespace Shop.Repos.Admin.Interfaces
{
	public interface ISpecialTagRepository
	{
		void Add(SpecialTag specialTag);
		void Update(SpecialTag specialTag);
		void Delete(int id);
		List<SpecialTag> GetAll();
		Task<IEnumerable<SpecialTag>> GetAllAsync();
		SpecialTag GetById(int id);
		void Save();
	}
}
