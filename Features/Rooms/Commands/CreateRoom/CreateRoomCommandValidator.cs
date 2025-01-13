using FluentValidation;

namespace ManagementSystem.Api.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Capacity)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);
        RuleFor(x => x.HourlyRate)
            .NotEmpty()
            .GreaterThan(0);
    }
}