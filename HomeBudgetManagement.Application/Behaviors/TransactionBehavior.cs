using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //return right away in case you want to terminate remaining process or don't want to proceed with all the pipeline or logic behind this pipeline
            //return default(TResponse); 

            try
            {
                //_unitOfWork.CreateTransaction();
                var response = await next();
                await _unitOfWork.SaveChangesAsync();
                return response;
            }
            catch (Exception)
            {

                _unitOfWork.Rollback();
            }

            //_unitOfWork.Commit();

            return default(TResponse);
        }
    }
}
