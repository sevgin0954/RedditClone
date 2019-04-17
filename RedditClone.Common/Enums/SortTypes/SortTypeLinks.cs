using System.Collections.Generic;

namespace RedditClone.Common.Enums.SortTypes
{
    public static class SortTypeLinks
    {
        public static Dictionary<string, string> SortTypeIconLink { get; private set; } = new Dictionary<string, string>()
        {
            { "Top", "<i class=\"fas fa-chart-line\"></i>" },
            { "Controversial", "<i class=\"fas fa-bolt\"></i>" },
            { "Best", "<i class=\"fas fa-rocket\"></i>" },
            { "New", "<i class=\"fas fa-certificate\"></i>" },
            { "Old", "<i class=\"far fa-clock\"></i>" }
        };
    }
}
