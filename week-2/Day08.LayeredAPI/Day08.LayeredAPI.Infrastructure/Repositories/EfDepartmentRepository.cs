using Microsoft.EntityFrameworkCore;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;
using Day08.LayeredAPI.Infrastructure.Data;

namespace Day08.LayeredAPI.Infrastructure.Repositories;

public class EfDepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public EfDepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Department> AddAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Entry(department).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department department)
    {
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Departments.AnyAsync(d => d.Id == id);
    }
}
