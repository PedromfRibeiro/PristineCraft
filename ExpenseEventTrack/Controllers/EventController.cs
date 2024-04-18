using Application.DTO.Event;
using Application.Helper;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Controllers;
using Persistence.Helper;
using Domain.Entities.Event;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection.Metadata.Ecma335;

namespace ExpenseEventTrack.Controllers;

public class EventController(IEventRepository _eventRepository, IMapper _mapper) : BaseApiController

{
	[HttpGet, Route("Fetches")]
	public async Task<ActionResult<PagedList<MinimalEventDto>>> Fetches([FromQuery] FilterParams filterParams)
	{
		var response = await _eventRepository.Fetch(filterParams);

		if (response.IsT0)
		{
			Response.AddPaginationHeader(response.AsT0.CurrentPage, response.AsT0.PageSize, response.AsT0.TotalCount, response.AsT0.TotalPages);
			return response.AsT0;
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpGet, Route("Fetch")]
	public async Task<ActionResult<EventDto>> Fetch(int Id)
	{
		var response = await _eventRepository.Fetch(Id);
		if (response.IsT0)
		{
			return response.AsT0;
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[Authorize]
	[HttpPost]
	public async Task<ActionResult<EventDto>> Create(CreateEventDto eventDto)
	{
		string email = User.Claims.Single(x => x.Type == ClaimTypes.Email).Value ?? "aa";

		Event record = _mapper.Map<Event>(eventDto);
		var response = await _eventRepository.Create(record);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPut]
	public async Task<ActionResult<EventDto>> Update(EventDto eventDto)
	{
		var response = await _eventRepository.Update(eventDto);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpDelete]
	public async Task<ActionResult> Delete(int id)
	{
		var response = await _eventRepository.Delete(id);

		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}
}