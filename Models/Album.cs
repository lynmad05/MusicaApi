using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MusicaApi.Models;

[Table("Album")]
[Index("Titulo", "Artista", Name = "UQ_Album_Titulo_Artista", IsUnique = true)]
public partial class Album
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100)]
    public string Titulo { get; set; } = null!;

    [Required(ErrorMessage = "El artista es obligatorio.")]
    [StringLength(100)]
    public string Artista { get; set; } = null!;

    [Required(ErrorMessage = "El género es obligatorio.")]
    [StringLength(50)]
    public string Genero { get; set; } = null!;

    [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
    public int AnioLanzamiento { get; set; }

    [JsonIgnore]
    [InverseProperty("Album")]
    public virtual ICollection<Cancion> Cancions { get; set; } = new List<Cancion>();
}
