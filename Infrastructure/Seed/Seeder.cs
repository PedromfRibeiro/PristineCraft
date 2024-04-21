﻿using PristineCraft.Domain.Entities;
using PristineCraft.Domain.Entities.Payee;
using Infrastructure;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Seed;

internal class Seeder
{
    internal static async Task<int> CreateCategories(DataContext _context)
    {
        var jsonData = File.ReadAllText("../Persistence/Seed/Resources/payee.json");
        var storeCategories = JsonConvert.DeserializeObject<Dictionary<string, List<Payee>>>(jsonData);
        if (storeCategories == null)
        {
            return 0;
        }
        foreach (var item in storeCategories.Keys)
        {
            List<Payee> ListPayee = storeCategories[item];
            var category = _context.DbPayeeCategory.Where(x => x.Name == item).FirstOrDefault();
            if (category == null)
            {
                _context.DbPayeeCategory.Add(new PayeeCategory() { Name = item });
                _context.SaveChanges();
                category = _context.DbPayeeCategory.Where(x => x.Name == item).First();
            }
            foreach (Payee payee in ListPayee)
            {
                payee.Category = category;
            }
            await _context.DbPayee.AddRangeAsync(ListPayee);
        }
        return _context.SaveChanges();
    }

    public static async Task<int> CreateBankAccount(DataContext _context)
    {
        var jsonData = File.ReadAllText("../Persistence/Seed/Resources/BankAccount.json");
        var items = JsonConvert.DeserializeObject<List<BankAccount>>(jsonData);
        if (items == null)
        {
            return 0;
        }
        foreach (var item in items)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, _context.DbUser.Count());
            item.Proprietary = _context.DbUser.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).FirstOrDefault();
            await _context.DbBankAccount.AddAsync(item);
        }
        return await _context.SaveChangesAsync();
    }
}