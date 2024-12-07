using MediatR;

namespace Application.Shared;

public class CommandHandlerBase(IMediator mediator)
{
    protected readonly IMediator Mediator = mediator;
}
