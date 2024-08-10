namespace Salvation.WriteSide.Models.Entities;

public class MainTable
{
    public Guid TaskID { get; set; }

    public string TaskCode { get; set; } = null!;

    public string Status { get; set; } = null!;
}
