using System;
using System.Collections.Generic;

namespace BTL.Data;

public partial class Lecture
{
    public int LectureId { get; set; }

    public string LectureName { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public string? VideoUrl { get; set; }

    public decimal? Rating { get; set; }

    public int? Views { get; set; }

    public int? ReviewCount { get; set; }

    public string? CourseInfo { get; set; }

    public int? CourseId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<LectureReview> LectureReviews { get; set; } = new List<LectureReview>();
}
