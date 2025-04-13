using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Admin.Interfaces;

public class SpecialTagRepository : ISpecialTagRepository
{
	private readonly ApplicationDbContext _context;

	public SpecialTagRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public void Add(SpecialTag specialTag)
	{
		_context.SpecialTags.Add(specialTag);
	}

	public void Update(SpecialTag specialTag)
	{
		_context.SpecialTags.Update(specialTag);
	}

	public void Delete(int id)
	{
		SpecialTag specialTag = GetById(id);
		if (specialTag != null)
		{
			_context.SpecialTags.Remove(specialTag);
		}
	}

	public List<SpecialTag> GetAll()
	{
		return _context.SpecialTags.ToList();
	}

	public async Task<IEnumerable<SpecialTag>> GetAllAsync()
	{
		return await _context.SpecialTags.ToListAsync();
	}

	public SpecialTag GetById(int id)
	{
		return _context.SpecialTags.FirstOrDefault(s => s.Id == id);
	}

	public void Save()
	{
		_context.SaveChanges();
	}
}
