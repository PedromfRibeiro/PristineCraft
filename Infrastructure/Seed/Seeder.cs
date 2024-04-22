using PristineCraft.Domain.Entities;
using PristineCraft.Domain.Entities.Payee;
using Infrastructure;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Seed;

internal class Seeder
{
    internal static async void CreateCategories(DataContext _context)
    {
        var jsonData = File.ReadAllText("../Infrastructure/Seed/Resources/payee.json");
        var storeCategories = JsonConvert.DeserializeObject<Dictionary<string, List<Payee>>>(jsonData);
        if (storeCategories == null)
            return;

        foreach (var item in storeCategories.Keys)
        {
            List<Payee> ListPayee = storeCategories[item];
            var category = _context.DbPayeeCategory.Where(x => x.Name == item).FirstOrDefault();
            if (category == null)
            {
                //_context.DbPayeeCategory.Add(new PayeeCategory() { Name = item });
                //_context.SaveChanges();
                category = new PayeeCategory() { Name = item };// _context.DbPayeeCategory.Where(x => x.Name == item).First();
            }
            foreach (Payee payee in ListPayee)
            {
                payee.Category = category;
            }
            await _context.DbPayee.AddRangeAsync(ListPayee);
        }
        _context.SaveChanges();
    }

    public static async void CreateBankAccount(DataContext _context)
    {
        var jsonData = File.ReadAllText("../Infrastructure/Seed/Resources/BankAccount.json");
        var items = JsonConvert.DeserializeObject<List<BankAccount>>(jsonData);
        if (items == null)
            return;

        foreach (var item in items)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, _context.DbUser.Count());
            item.Proprietary = _context.DbUser.OrderBy(r => r.Id).Skip(toSkip).Take(1).FirstOrDefault();
            await _context.DbBankAccount.AddAsync(item);
        }
        _context.SaveChanges();
    }
}