using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Models
{
    public class BlogTagList<T> : List<T>
    {
        public BlogTagList(List<T> items)
        {
            this.AddRange(items);
        }

        public static async Task<BlogTagList<T>> CreateAsync(IQueryable<T> source)
        {
            var items = await source.ToListAsync();

            return new BlogTagList<T>(items);
        }
    }
}