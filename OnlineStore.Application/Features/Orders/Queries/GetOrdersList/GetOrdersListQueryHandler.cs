using AutoMapper;
using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Core.Enums;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Core.Entities.Order> orders;

        if (request.Status.HasValue)
        {
            orders = await _orderRepository.GetByStatusAsync(request.Status.Value);
        }
        else if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            orders = await _orderRepository.GetOrdersByDateRangeAsync(request.StartDate.Value, request.EndDate.Value);
        }
        else
        {
            orders = await _orderRepository.GetAllAsync();
        }

        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}

