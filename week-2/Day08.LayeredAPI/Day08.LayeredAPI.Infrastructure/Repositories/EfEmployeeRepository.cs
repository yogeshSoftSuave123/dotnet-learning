using Microsoft.EntityFrameworkCore;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;
using Day08.LayeredAPI.Infrastructure.Data;

namespace Day08.LayeredAPI.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IEmployeeRepository
/// Replaces in-memory implementation with database persistence
/// </summary>
public class EfEmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EfEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)  // Eager loading to avoid N+1
            .AsNoTracking()  // Read-only query for better performance
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
}
