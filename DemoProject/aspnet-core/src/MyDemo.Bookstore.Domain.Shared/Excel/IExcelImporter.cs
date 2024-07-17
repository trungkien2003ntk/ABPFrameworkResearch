using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace MyDemo.BookStore.Excel
{
    public interface IExcelImporter
    {
        Task ImportAsync<TTempEntity, TKey>(MemoryStream memoryStream)
            where TTempEntity : class, IEntity<TKey>;

        void SetColumnDefinition(List<IExcelColumnDefinition> columnDefinitions);
    }
}