using System;
using System.Collections.Generic;

namespace PartialZ.DataAccess.PartialZDB;

public partial class EmailTemplate
{
    public int Id { get; set; }

    public string? Template { get; set; }

    public string? Subject { get; set; }

    public string? Description { get; set; }

    public int? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastModifedDate { get; set; }
}
