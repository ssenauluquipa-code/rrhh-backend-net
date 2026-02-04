using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Departament;
using Rrhh_backend.Presentation.DTOs.Responses.Departament;
using System.Diagnostics.CodeAnalysis;

namespace Rrhh_backend.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _reporDepartment;
        public DepartmentService(IDepartmentRepository reporDepartment)
        {
            _reporDepartment = reporDepartment;
        }

        public async Task<bool> CreateAsync(DepartamentRequest request)
        {
            var dept = new Department
            {
                DepartmentName = request.DepartmentName,
                CompanyId = request.CompanyId,
            };
            await _reporDepartment.CreateAsync(dept);
            await _reporDepartment.SaveChangeAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int companyId)
        {
            var dept = await _reporDepartment.GetByIdAsync(id, companyId);
            if (dept == null) return false;
            dept.IsActive = false;
            await _reporDepartment.SaveChangeAsync();
            return true;
        }

        public async Task<IEnumerable<DepartamentResponse>> GetAll()
        {
            var data = await _reporDepartment.GetAll();
            return data.Select(d => new DepartamentResponse
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                CompanyId = d.CompanyId,
                IsActive = d.IsActive
            });
        }

        public async Task<IEnumerable<DepartamentResponse>> GetListAsync(int companyId)
        {
            var data = await _reporDepartment.GetAllByCompanyAsync(companyId);
            return data.Select(d => new DepartamentResponse
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                CompanyId = d.CompanyId,
                IsActive = d.IsActive
            });
        }

        public async Task<bool> UpdateAsync(int id, DepartamentRequest request)
        {
            var dept = await _reporDepartment.GetByIdAsync(id, request.CompanyId);
            if (dept == null) return false;
            await _reporDepartment.UpdateAsync(dept);
            await _reporDepartment.SaveChangeAsync();
            return true;
        }       
    }
}
