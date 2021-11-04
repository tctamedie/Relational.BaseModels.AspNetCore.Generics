﻿using Microsoft.Extensions.Options;
using Relational.BaseModels.AspNetCore.Generics.Models;
using Relational.BaseModels.AspNetCore.Generics.Models.Security;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Relational.BaseModels.AspNetCore.Generics.Services.Security
{
    public interface  ISystemUserService<TContext> : IStandardModifierCheckerService<SystemUser, SystemUserDto, string, TContext, StandardStatusFilter>
        where TContext: SecurityContext
    {

    }

    public class SystemUserService<TContext> : StandardModifierCheckerService<SystemUser, SystemUserDto, string, TContext, StandardStatusFilter>, ISystemUserService<TContext>
        where TContext: SecurityContext
    {
        IEncryptionRepository encryptionRepository;
        SecurityOption securityOption;
        public SystemUserService(TContext db, IEncryptionRepository encryptionRepository, IOptions<SecurityOption> options) : base(db)
        {
            securityOption = options.Value;
            this.encryptionRepository = encryptionRepository;
        }
        public override async Task<OutputModel> AddAsync(SystemUserDto row, string createBy)
        {
            var output = Validate(row);
            if (output.Error)
                return output;
            row.Password = encryptionRepository.Encrypt(row.Password.Trim(), securityOption.Password);
            return await base.AddAsync(row, createBy);
        }
        protected override OutputModel Validate(SystemUserDto row, [CallerMemberName] string caller = "")
        {
            if (caller.ToLower().StartsWith("add"))
            {
                if (string.IsNullOrEmpty(row.Password))
                {
                    return new OutputModel(true)
                    {
                        Message = "Password cannot be blank"
                    };
                }
            }
            return base.Validate(row, caller);
        }
        public override Task<OutputModel> UpdateAsync(SystemUserDto row, string updatedBy)
        {
            //allow leaving passwords blank when updating user but maintain the existing password
            if (string.IsNullOrEmpty(row.Password))
            {
                var record = Find(row.Id);
                row.Password = record.Password;
            }
            return base.UpdateAsync(row, updatedBy);
            
        }
        

    }
}
