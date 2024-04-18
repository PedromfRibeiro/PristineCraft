using Application.Helper;
using OneOf.Types;
using OneOf;
using Domain.Entities.Event;
using Application.DTO.Event;

namespace Application.Interfaces;

public interface IEventRepository
{
	Task<OneOf<bool, Error<string>>> Create(Event record);

	Task<OneOf<bool, Error<string>>> Delete(int Id);

	Task<OneOf<EventDto, Error<string>>> Fetch(int Id);

	Task<OneOf<PagedList<MinimalEventDto>, NotFound>> Fetch(FilterParams filterParams);

	Task<OneOf<bool, Error<string>>> Update(EventDto eventDto);
}