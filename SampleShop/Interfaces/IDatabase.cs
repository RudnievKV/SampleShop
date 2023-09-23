using System.Collections.Generic;
using SampleShop.Model;

namespace SampleShop.Interfaces
{
    public interface IDatabase
    {
        IDictionary<int, Item> Items { get; }
        IDictionary<int, Order> Orders { get; }
    }
}