using System;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.BookStore.ExpenseCodes;

public interface IExpenseCodeRepository : IRepository<ExpenseCode, Guid>
{
}
