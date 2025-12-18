using AutoMapper;
using MediatR;
using OnlineStore.Application.Features.Orders.DTOs;
using OnlineStore.Core.Interfaces;

namespace OnlineStore.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(request.Id);
        if (order == null)
        {
            return null;
        }

        var orderDto = _mapper.Map<OrderDto>(order);
        return orderDto;
    }
}

