using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IAnswerRepository : IBaseEntityRepository<long, Answer, CreateAnswerDlDto, UpdateAnswerDlDto>
{ }
