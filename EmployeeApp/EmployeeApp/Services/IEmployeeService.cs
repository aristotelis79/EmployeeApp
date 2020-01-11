﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Data.Entities;

namespace EmployeeApp.Services
{
    public interface IEmployeeService
    {
        Task<Employee> GetById(Guid id, CancellationToken token = default);
        Task<List<Employee>> GetAll(CancellationToken token = default);
        Task<Employee> InsertAsync(Employee employee,  CancellationToken token = default);
        Task<bool> DeleteAsync(Employee entity, CancellationToken token);
        Task<bool> UpdateAsync(Employee entity, CancellationToken token);
    }
}