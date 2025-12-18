using AutoMapper;
using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Queries.GetClientOrders;

public class GetClientOrdersQueryHandler : IRequestHandler<GetClientOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetClientOrdersQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetClientOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByClientIdAsync(request.ClientId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}

