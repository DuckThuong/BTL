using System;
using System.Collections.Generic;

namespace BTL.Data;

public partial class SearchResult
{
    public int SearchId { get; set; }

    public string SearchKeyword { get; set; } = null!;

    public DateTime? SearchDateTime { get; set; }

    public string? Results { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
