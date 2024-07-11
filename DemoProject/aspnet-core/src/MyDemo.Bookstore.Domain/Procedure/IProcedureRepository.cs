using System.Threading.Tasks;

namespace MyDemo.BookStore.Procedure;

public interface IProcedureRepository
{
    public Task RunProcedure();
}
