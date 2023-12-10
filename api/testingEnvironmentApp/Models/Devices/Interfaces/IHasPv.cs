namespace testingEnvironmentApp.Models.Devices.Interfaces
{
    /// <summary>
    /// Interfejs IHasPv definiuje właściwość Pv (wartość procesu).
    /// </summary>
    public interface IHasPv
    {
        /// <summary>Wartość procesu dla urządzenia.</summary>
        public double Pv { get; set; }
    }
}
