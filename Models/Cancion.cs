using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MusicaApi.Models;

[Table("Cancion")]
[Index("AlbumId", "NumeroPista", Name = "UQ_Cancion_Album_Pista", IsUnique = true)]
public partial class Cancion
{
    [Key]
    public int Id { get; set; }

    public int AlbumId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100)]
    public string Titulo { get; set; } = null!;

    [Range(1, 100, ErrorMessage = "El número de pista debe estar entre 1 y 100")]
    public int NumeroPista { get; set; }

    [Range(1, 7200, ErrorMessage = "La duración debe estar entre 1 y 7200 segundos.")]
    public int DuracionSegundos { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Las reproducciones no pueden ser negativas.")]
    public int Reproducciones { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Cancions")]
    public virtual Album? Album { get; set; } = null!;
}
