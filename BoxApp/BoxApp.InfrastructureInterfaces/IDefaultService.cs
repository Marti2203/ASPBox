using System.Collections.Generic;
using BoxApp.CommonFiles.DTO;

namespace BoxApp.Infrastructure
{
    public interface IDefaultService<DTOType> where DTOType:DTO
    {
        void Delete(int id);
        void Edit(DTOType dto);
        BoxDTO Get(int id);
        IEnumerable<DTOType> GetAll();
        void Insert(DTOType box);
    }
}