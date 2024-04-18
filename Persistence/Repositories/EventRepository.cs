using Application.DTO.Event;
using Application.Exception;
using Application.Helper;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Domain.Entities;
using Domain.Entities.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using System.Security.Claims;

namespace Persistence.Repositories;

public class EventRepository : IEventRepository
{
	private readonly IMapper mapper;
	private readonly DataContext context;
	private readonly IHttpContextAccessor httpContextAccessor;
	private string email;

	public EventRepository(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
	{
		this.mapper = mapper;
		this.context = context;
		this.httpContextAccessor = httpContextAccessor;
		email = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
	}

	public async Task<OneOf<bool, Error<string>>> Create(Event record)
	{
		OneOf<User, NotFound> accountFindEmail = await AccountFindByEmail(email);
		if (accountFindEmail.IsT1)
			return new Error<string>(ExceptionStrings.Server_Failed);
		if (record.SubCategoryId != null)
			record.CategoryId = context.DbEventSubCategory.FindAsync(record.SubCategoryId).Result?.CategoryId;

		record.Owner = accountFindEmail.AsT0;
		context.DbEvent.Add(record);
		return await context.SaveChangesAsync() > 0 ? true : new Error<string>(ExceptionStrings.Server_Create_Failed);
	}

	public async Task<OneOf<bool, Error<string>>> Delete(int Id)
	{
		var record = await context.DbEvent.Where(x => x.Id == Id).SingleOrDefaultAsync();
		if (record == null || record is default(Event))
			return new Error<string>(ExceptionStrings.Record_NotFound);
		context.Entry(record).State = EntityState.Deleted;
		return await context.SaveChangesAsync() > 0 ? true : new Error<string>(ExceptionStrings.Server_Delete_Failed);
	}

	public async Task<OneOf<EventDto, Error<string>>> Fetch(int Id)
	{
		var record = await context.DbEvent.Where(x => x.Id == Id).ProjectTo<EventDto>(mapper.ConfigurationProvider).SingleOrDefaultAsync();

		if (record == null || record is default(EventDto))
			return new Error<string>(ExceptionStrings.Record_NotFound);
		return record;
	}

	public async Task<OneOf<PagedList<MinimalEventDto>, NotFound>> Fetch(FilterParams filterParams)
	{
		var query = context.DbEvent.ProjectTo<MinimalEventDto>(mapper.ConfigurationProvider).AsNoTracking();
		return await PagedList<MinimalEventDto>.CreateAsync(query, filterParams._pageNumber, filterParams.PageSize, filterParams.FilterOptions2);
	}

	public async Task<OneOf<bool, Error<string>>> Update(EventDto eventDto)
	{
		var record = await context.DbEvent.Where(x => x.Id == eventDto.Id).SingleOrDefaultAsync();
		if (record == null || record is default(Event))
			return new Error<string>(ExceptionStrings.Record_NotFound);
		mapper.Map(eventDto, record);
		context.Entry(record).State = EntityState.Modified;
		return await context.SaveChangesAsync() > 0 ? true : new Error<string>(ExceptionStrings.Server_Update_Failed);
	}

	internal async Task<OneOf<User, NotFound>> AccountFindByEmail(string email)
	{
		User? user = await context.DbUser.Where(x => x.NormalizedEmail == email.ToUpper()).SingleOrDefaultAsync();
		if (user == null || user == default(User))
			return new NotFound();
		return user;
	}
}