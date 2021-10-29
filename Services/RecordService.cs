using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Relational.BaseModels.AspNetCore.Generics.Services
{
    using Annotations;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// This service provides for any database entity
    /// </summary>
    /// <typeparam name="TEntity">Entity that has been mapped to a database</typeparam>
    /// <typeparam name="TMap">Data Transfer Object</typeparam>
    /// <typeparam name="T">data type of the primary key</typeparam>
    /// <typeparam name="TDbContext">The database context to use for persistence, retrieval and deletion</typeparam>
    public interface IRecordService<TEntity, TMap, T, TDbContext> : IAnnotationService<TEntity, TMap, T>
        where TEntity : Record<T>
        where TMap : RecordDto<T>
        where T : IEquatable<T>

        where TDbContext : DbContext
    {
        Task<OutputModel> GetAsync(Expression<Func<TEntity, bool>> match);
        Task<OutputModel> AddAsync(TMap row, string createdBy);
        Task<OutputModel> DeleteAsync(T id, string deletedBy);
        /// <summary>
        /// Fetches records based on search criterion
        /// </summary>
        /// <param name="match">Search Criterion</param>
        /// <returns>Result of the fetching which may include data if successful otherwise an error Message is returned</returns>
        Task<OutputModel> GetAllAsync(Expression<Func<TEntity, bool>> match = null);
        Task<OutputModel> GetAsync(T id);
        Task<OutputModel> UpdateAsync(TMap row, string updatedBy);
        /// <summary>
        /// Validates deletion of a given record on condition that it was created by the the user that wants to delete it
        /// </summary>
        /// <param name="id">primary key of the record to be deleted</param>
        /// <param name="user">the user who wants to delete the record</param>
        /// <returns></returns>
        bool ValidateDeleteOnCreator(T id, string user);

    }
    public abstract class RecordService<TEntity, TMap, T, TDbContext> : AnnotationService<TEntity, TMap, T>, IRecordService<TEntity, TMap, T, TDbContext>
        where TEntity : Record<T>
        where TMap : RecordDto<T>
        where T : IEquatable<T>
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;
        //private readonly IAuditTrailService _auditTrailService;
        protected string _tableHeader;
        protected string _modelHeader;
        public RecordService(TDbContext context
            //, IAuditTrailService auditTrailService
            )
        {
            _context = context;
            //_auditTrailService = auditTrailService;
        }

        public async Task<OutputModel> GetAsync(T id)
        {
            var row = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id))                ;

            return new OutputModel
            {
                Data = row
            };
        }
        public async Task<OutputModel> GetAsync(Expression<Func<TEntity, bool>> match)
        {
            var row = await _context.Set<TEntity>().FirstOrDefaultAsync(match);
            return new OutputModel
            {
                Data = row
            };
        }

        public async Task<OutputModel> GetAllAsync(Expression<Func<TEntity, bool>> match = null)
        {
            var rows = new List<TEntity>();
            if (match == null)
                rows = await _context.Set<TEntity>().ToListAsync();
            else
                rows = await _context.Set<TEntity>().Where(match).ToListAsync();
            return new OutputModel
            {
                Data = rows
            };
        }
        /// <summary>
        /// Validates deletion of a given record on condition that it was created by the the user that wants to delete it
        /// </summary>
        /// <param name="id">primary key of the record to be deleted</param>
        /// <param name="user">the user who wants to delete the record</param>
        /// <returns> true if the record can be deleted otherwise false</returns>
        public virtual bool ValidateDeleteOnCreator(T id, string user)
        {
            return true;
        }
        /// <summary>
        /// Validates deletion of a given record on condition that it was modified by the the user that wants to delete it
        /// </summary>
        /// <param name="id">primary key of the record to be deleted</param>
        /// <param name="user">the user who wants to delete the record</param>
        /// <returns>true if the record can be deleted otherwise false</returns>
        protected virtual bool ValidateDeleteOnModifier(T id, string user)
        {
            return true;
        }
        protected virtual OutputModel Validate(TMap row, [CallerMemberName] string caller = "")
        {
            if (caller.ToLower().StartsWith("add"))
            {
                if (Any(s => s.Id.Equals(row.Id)))
                {
                    return new OutputModel
                    {
                        Error = true,
                        Message = $" Key {row.Id}for {_modelHeader} already exist"
                    };
                }
            }
            if (caller.ToLower().StartsWith("update"))
            {
                if (!Any(s => s.Id.Equals(row.Id)))
                {
                    return new OutputModel
                    {
                        Error = true,
                        Message = $" Key {row.Id}for {_modelHeader} does not exist"
                    };
                }
            }
            return new OutputModel();

        }
        /// <summary>
        /// Validates approval of a given record on condition that it was not created by the the user that wants to approval it
        /// </summary>
        /// <param name="id">primary key of the record to be approved</param>
        /// <param name="user">the user who wants to approve the record</param>
        /// <returns>true if approval is allowed otherwise false</returns>
        protected virtual bool ValidateAuthoriseOnCreator(T id, string user)
        {
            return true;
        }
        /// <summary>
        /// Validates approval of a given record on condition that it was not modified by the the user that wants to approval it
        /// </summary>
        /// <param name="id">primary key of the record to be approved</param>
        /// <param name="user">the user who wants to approve the record</param>
        /// <returns>true if approval is allowed otherwise false</returns>
        protected virtual bool ValidateAuthoriseOnModifier(T id, string user)
        {
            return true;
        }

        protected virtual OutputModel Validate(T id, string user, [CallerMemberName] string caller = "")
        {
            if (!Any(s => s.Id.Equals(id)))
            {
                return new OutputModel
                {
                    Error = true,
                    Message = $" Key {id}for {_modelHeader} does not exist"
                };
            }
            if (caller.ToLower().StartsWith("delete"))
            {
                var validateCreator = ValidateDeleteOnCreator(id, user);
                var validateModifier = ValidateDeleteOnModifier(id, user);
                if (!(validateCreator || validateModifier))
                {
                    return new OutputModel
                    {
                        Error = true,
                        Message = $" {_modelHeader} was created by a different user. Deletion failed"
                    };
                }
            }
            if (caller.ToLower().StartsWith("approve"))
            {
                var validateCreator = ValidateAuthoriseOnCreator(id, user);
                var validateModifier = ValidateAuthoriseOnModifier(id, user);
                if (!(validateCreator || validateModifier))
                {
                    return new OutputModel
                    {
                        Error = true,
                        Message = $" {_modelHeader} was created by a different user. Approval failed"
                    };
                }
            }


            return new OutputModel();

        }
        protected TTarget CreateTarget<TTarget, TSource>(TSource source)
        {
            var config = new MapperConfiguration(s => s.CreateMap<TSource, TTarget>());
            var mapper = config.CreateMapper();
            return mapper.Map<TTarget>(source);
        }
        protected List<TTarget> CreateTarget<TTarget, TSource>(List<TSource> source)
        {
            var config = new MapperConfiguration(s => s.CreateMap<TSource, TTarget>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TTarget>>(source);
        }
        protected virtual void AppendCreator(TEntity row, string createdBy)
        {

        }
        protected virtual void AppendAuthoriser(TEntity row, string authorisedBy)
        {

        }
        protected bool Any(Expression<Func<TEntity, bool>> match)
        {
            return _context.Set<TEntity>().Any(match);
        }
        protected virtual void AppendModifier(TEntity row, string updatedBy)
        {

        }
        public async Task<OutputModel> AddAsync(TMap row, string createBy)
        {
            //********* start validations *********************
            var validation = Validate(row);
            if (validation.Error)
                return validation;
            //********* end validations *********************
            var record = CreateTarget<TEntity, TMap>(row);
            AppendCreator(record, createBy);
            await _context.Set<TEntity>().AddAsync(record);
            await _context.SaveChangesAsync();

            //await _auditTrailService.SendAsync(new AuditTrailDTO
            //{
            //    ActionType = "CREATE",
            //    AfterImage = DataImageBuilder.BuildDataImageRow(newRow).ToString(),
            //    AuditDate = DateTime.UtcNow.AddHours(2),
            //    BeforeImage = string.Empty,
            //    DataTable = "IndicatorGroups",
            //    UserId = Country.CreatedBy
            //});

            return new OutputModel
            {
                Message = "Successfully Added Record"
            };
        }

        public virtual async Task<OutputModel> UpdateAsync(TMap row, string updatedBy)
        {
            //********* start validations *********************
            var validation = Validate(row);
            if (validation.Error)
                return validation;
            //********* end validations *********************
            var rowToUpdate = await _context.Set<TEntity>().FirstOrDefaultAsync(s => s.Id.Equals(row.Id));
            if (rowToUpdate is null)
            {
                return new OutputModel(true)
                {

                    Message = $"{_modelHeader} does not exists, update failed"
                };
            }
            //string beforeImage = DataImageBuilder.BuildDataImageRow(rowToUpdate).ToString();
            rowToUpdate.AssignPropertiesForm(row);
            AppendModifier(rowToUpdate, updatedBy);
            await _context.SaveChangesAsync();

            //await _auditTrailService.SendAsync(new AuditTrailDTO
            //{
            //    ActionType = "EDIT",
            //    AfterImage = DataImageBuilder.BuildDataImageRow(rowToUpdate).ToString(),
            //    AuditDate = DateTime.UtcNow.AddHours(2),
            //    BeforeImage = beforeImage,
            //    DataTable = "IndicatorGroups",
            //    UserId = Country.CreatedBy
            //});

            return new OutputModel
            {

                Message = "Successfully updated record"
            };
        }

        public async Task<OutputModel> DeleteAsync(T id, string deletedBy)
        {
            var row = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            /****** start validations ****/
            var validation = Validate(id, deletedBy);
            if (validation.Error)
                return validation;
            /****** end validations ****/

            _context.Set<TEntity>().Remove(row);
            await _context.SaveChangesAsync();

            //await _auditTrailService.SendAsync(new AuditTrailDTO
            //{
            //    ActionType = "DELETE",
            //    AfterImage = string.Empty,
            //    AuditDate = DateTime.UtcNow.AddHours(2),
            //    BeforeImage = DataImageBuilder.BuildDataImageRow(row).ToString(),
            //    DataTable = "IndicatorGroups",
            //    UserId = deletedBy
            //});

            return new OutputModel
            {
                Message = "Successfully Deleted record"
            };
        }

        public async Task<OutputModel> ApproveAsync(T id, string approvedBy)
        {
            /****** start validations ****/
            var validation = Validate(id, approvedBy);
            if (validation.Error)
                return validation;
            /****** end validations ****/
            var row = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            AppendAuthoriser(row, approvedBy);
            _context.Set<TEntity>().Remove(row);
            await _context.SaveChangesAsync();

            //await _auditTrailService.SendAsync(new AuditTrailDTO
            //{
            //    ActionType = "DELETE",
            //    AfterImage = string.Empty,
            //    AuditDate = DateTime.UtcNow.AddHours(2),
            //    BeforeImage = DataImageBuilder.BuildDataImageRow(row).ToString(),
            //    DataTable = "IndicatorGroups",
            //    UserId = deletedBy
            //});

            return new OutputModel
            {
                Message = "Successfully approved record"
            };
        }
    }
}
