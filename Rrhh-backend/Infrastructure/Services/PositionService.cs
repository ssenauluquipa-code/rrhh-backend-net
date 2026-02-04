using Rrhh_backend.Core.Entities;
using Rrhh_backend.Core.Interfaces.Repositories;
using Rrhh_backend.Core.Interfaces.Services;
using Rrhh_backend.Presentation.DTOs.Requests.Position;
using Rrhh_backend.Presentation.DTOs.Responses.Position;

namespace Rrhh_backend.Infrastructure.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        public PositionService(IPositionRepository positionRepository) {
            _positionRepository = positionRepository;
        }

        public async Task<bool> ChangeStatusAsync(int id, int companyId, bool status)
        {
            var entity = await _positionRepository.GetByIdAsync(id, companyId);
            if (entity == null) return false;
            entity.IsActive = status;
            await _positionRepository.SaveChangeAsync();
            return true;
        }

        public async Task<bool> CreateAsync(PositionRequest request)
        {
            var entity = new Position
            {
                PositionName = request.PositionName,
                DepartmentId = request.DepartmentId,
                CompanyId = request.CompanyId,
                BaseSalary = request.BaseSalary,
                IsActive = true
            };
            await _positionRepository.CreateAsync(entity);
            await _positionRepository.SaveChangeAsync();
            return true;
        }

        public async Task<IEnumerable<PositionResponse>> GetAllAsync()
        {
            var data = await _positionRepository.GetAllPosition();
            return MapList(data);
        }

        public async Task<IEnumerable<PositionResponse>> GetByCompanyAsync(int companyId)
        {
            var data = await _positionRepository.GetByCompanyAsync(companyId);
            return MapList(data);
        }

        public async Task<bool> UpdateAsync(int id, PositionRequest request)
        {
            var entity = await _positionRepository.GetByIdAsync(id, request.CompanyId);
            if (entity == null) return false;

            entity.PositionName = request.PositionName;
            entity.DepartmentId = request.DepartmentId;
            entity.BaseSalary = request.BaseSalary;

            await _positionRepository.UpdateAsync(entity);
            await _positionRepository.SaveChangeAsync();
            return true;
        }
        // Helper para no repetir código de mapeo
        private IEnumerable<PositionResponse> MapList(IEnumerable<Position> data) =>
            data.Select(p => new PositionResponse
            {
                PositionId = p.PositionId,
                PositionName = p.PositionName,
                DepartmentId = p.DepartmentId,
                DepartmentName = p.Department?.DepartmentName,
                CompanyId = p.CompanyId,
                BaseSalary = p.BaseSalary,
                IsActive = p.IsActive
            });
    }
}
