using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dccportal.org.Entities;
using dccportal.org.Interface;

namespace dccportal.org.Repository
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAccountRepository AccountRepository => new AccountRepository(_context, _mapper);
        public IBelieverRepository BelieverRepository => new BelieverRepository(_context, _mapper);
        public IMemberRepository MemberRepository => new MemberRepository(_context, _mapper);
        public ISettingsRepository SettingsRepository => new SettingsRepository(_context, _mapper);
        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_context, _mapper);
        public IUnitRepository UnitRepository => new UnitRepository(_context, _mapper);
        public IAttendanceRepository AttendanceRepository => new AttendanceRepository(_context, _mapper);
        public IFinanceRepository FinanceRepository => new FinanceRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}