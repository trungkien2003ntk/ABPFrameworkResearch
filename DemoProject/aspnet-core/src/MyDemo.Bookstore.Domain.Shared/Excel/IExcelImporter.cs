using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MyDemo.BookStore.Excel
{
    public interface IExcelImporter
    {
        Task ImportAsync<TEntity>(MemoryStream memoryStream)
            where TEntity : class, IEntity;

        void SetColumnDefinition(List<IExcelColumnDefinition> columnDefinitions);
    }
}