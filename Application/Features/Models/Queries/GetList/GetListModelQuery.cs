using Core.Application.Requests;
using Core.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Queries.GetList
{
    public class GetListModelQuery:IRequest<GetListResponse<GetListModelListItemDto>>
    {
        public PageRequest PageRequest { get; set; }
        public class GetListModelQueryHandler:IRequestHandler<GetListModelQuery, GetListResponse<GetListModelListItemDto>> 
        {
           
            public Task<GetListResponse<GetListModelListItemDto>> Handle(GetListModelQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
