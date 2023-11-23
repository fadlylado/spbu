using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace spbu.Models;

public class DataKendaraan
{
    [Key]
    [JsonIgnore]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [StringLength(256)]
    public string? Id { get; set; }
    public int JumlahBbm { get; set; }

    [StringLength(20)]
    public string? NomorPlat { get; set; }

    public DateTime? TanggalPengisian { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public string? Createdby { get; set; }
    public string? Modifiedby { get; set; }


}
