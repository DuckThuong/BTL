using System;
using System.Collections.Generic;

namespace BTL.Data;

public partial class LectureReview
{
    public int ReviewId { get; set; }

    public int LectureId { get; set; }

    public int UserId { get; set; }

    public decimal? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Lecture Lecture { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
