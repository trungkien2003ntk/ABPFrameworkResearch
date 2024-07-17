using MyDemo.BookStore.Categories;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MyDemo.BookStore.SystemCategories;
public interface ISystemCategoryRepository : IRepository<SystemCategory, Guid>
{
    Task<SystemCategory> GetByName(string name);

}
