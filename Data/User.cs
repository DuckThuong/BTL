using System;
using System.Collections.Generic;

namespace BTL.Data;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<LectureReview> LectureReviews { get; set; } = new List<LectureReview>();

    public virtual ICollection<SearchResult> SearchResults { get; set; } = new List<SearchResult>();
}
