﻿using System;
using System.Collections.Generic;

namespace BTL.Data;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Gender { get; set; }

    public int? BirthYear { get; set; }

    public int? Age { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Status { get; set; }

    public string? Nationality { get; set; }

    public string? Occupation { get; set; }

    public string? Bio { get; set; }

    public string? Interests { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<LectureReview> LectureReviews { get; set; } = new List<LectureReview>();

    public virtual ICollection<SearchResult> SearchResults { get; set; } = new List<SearchResult>();
}
