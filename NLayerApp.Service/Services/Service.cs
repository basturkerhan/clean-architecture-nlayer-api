using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;
using System.Linq.Expressions;

namespace NLayerApp.Service.Services
{
    public class Service<Entity, Dto> : IService<Entity, Dto> where Entity : BaseEntity where Dto : class
    {
        private readonly IGenericRepository<Entity> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public Service(IGenericRepository<Entity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResponseDto<Dto>> AddAsync(Dto dto)
        {
            Entity entity = _mapper.Map<Entity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            // ef core id değerini entitye otomatik olarak atayacak
            return CustomResponseDto<Dto>.Success(StatusCodes.Status201Created, _mapper.Map<Dto>(entity));
        }

        public async Task<CustomResponseDto<IEnumerable<Dto>>> AddRangeAsync(IEnumerable<Dto> dtos)
        {
            List<Entity> entities = _mapper.Map<List<Entity>>(dtos);
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<IEnumerable<Dto>>.Success(StatusCodes.Status201Created, _mapper.Map<IEnumerable<Dto>>(entities));
        }

        public async Task<CustomResponseDto<bool>> AnyAsync(Expression<Func<Entity, bool>> expression)
        {
            bool result = await _repository.AnyAsync(expression);

            return CustomResponseDto<bool>.Success(StatusCodes.Status200OK, result);
        }

        public async Task<CustomResponseDto<IEnumerable<Dto>>> GetAllAsync()
        {
            List<Entity> entities = await _repository.GetAll().ToListAsync();

            return CustomResponseDto<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, _mapper.Map<IEnumerable<Dto>>(entities));
        }

        public async Task<CustomResponseDto<Dto>?> GetByIdAsync(int id)
        {
            Entity? entity = await _repository.GetByIdAsync(id);

            return CustomResponseDto<Dto>.Success(StatusCodes.Status200OK, _mapper.Map<Dto>(entity));
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id)
        {
            Entity? entity = await _repository.GetByIdAsync(id);
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            List<Entity> entities = await _repository.Where(x => ids.Contains(x.Id)).ToListAsync();
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(Dto dto)
        {
            _repository.Update(_mapper.Map<Entity>(dto));
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression)
        {
            List<Entity> entities = await _repository.Where(expression).ToListAsync();

            return CustomResponseDto<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, _mapper.Map<IEnumerable<Dto>>(entities));
        }


    }
}
