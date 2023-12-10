using System.ComponentModel.DataAnnotations.Schema;

namespace testingEnvironmentApp.Models.Interfaces
{
    /// <summary>
    /// Interfejs IHistorable określa, czy czujnik powinien być historiowany.
    /// </summary>
    public interface IHistorable
    {
        /// <summary>Określa, czy czujnik jest historiowany.</summary>
        [NotMapped]
        public bool ChannelIsHistorable { get; set; }

    }
}
