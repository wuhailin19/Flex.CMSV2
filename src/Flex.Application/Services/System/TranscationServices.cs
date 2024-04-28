namespace Flex.Application.Services
{
    public class TranscationServices : BaseService, ITranscationServices
    {
        public TranscationServices(IUnitOfWork unitOfWork, IMapper mapper, IdWorker idWorker, IClaimsAccessor claims)
            : base(unitOfWork, mapper, idWorker, claims)
        {
        }
        public bool InsertAsync()
        {
            var model = new TestTranscation()
            {
                Id = _idWorker.NextId(),
                Name = "张三",
                Password = "123456"
            };
            var result = _unitOfWork.GetRepository<TestTranscation>().InsertAsync(model, default(CancellationToken));
            _unitOfWork.SaveChanges();
            return result.IsCompletedSuccessfully;
        }
        public Task Insert()
        {
            var model = new List<TestTranscation>()
            {
                new TestTranscation{
                    Id = _idWorker.NextId(),
                    Name = "张三",
                    Password = "123456"
                },
                new TestTranscation{
                    Id = _idWorker.NextId(),
                    Password = "123456"
                }
            };
            var result = _unitOfWork.GetRepository<TestTranscation>().InsertAsync(model);
            _unitOfWork.SaveChanges();
            return result;
        }
        public ProblemDetails<string> TestAddTranscation()
        {

            _unitOfWork.SetTransaction();
            try
            {
                var model = new TestTranscation()
                {
                    Id = _idWorker.NextId(),
                    Name = "张三",
                    Password = "123456"
                };
                _unitOfWork.GetRepository<TestTranscation>().Insert(model);
                var model1 = new TestTranscation()
                {
                    Id = _idWorker.NextId(),
                    Password = "123456"
                };
                _unitOfWork.GetRepository<TestTranscation>().Insert(model1);
                var model2 = new TestTranscation()
                {
                    Id = _idWorker.NextId(),
                    Name = "张三"
                };
                _unitOfWork.GetRepository<TestTranscation>().Insert(model2);
                _unitOfWork.SaveChangesTran();
                //tran.Commit();
                return Problem<string>(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return Problem<string>(HttpStatusCode.BadRequest, JsonHelper.ToJson(ex));
            }
        }
    }
}
