using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Data
{
    public interface IContext<TData, TDescriptive, K>
    {
        Task CreateTableAsync();
        Task SetAsync(K key, TData data);
        Task<TData> ReadDataAsync(K key);
        Task<List<TDescriptive>> ReadAllAsync();
        Task DeleteAsync(K key);
    }
}