using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Data
{
    public interface IContext<TData, TDescriptive, K>
    {
        Task CreateTable();
        Task Set(K key, TData data);
        Task<TData> ReadData(K key);
        Task<List<TDescriptive>> ReadAll();
        Task Delete(K key);
    }
}