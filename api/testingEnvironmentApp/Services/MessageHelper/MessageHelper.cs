using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using testingEnvironmentApp.Services.MessageHelper.Interfaces;

namespace testingEnvironmentApp.Services.MessageHelper
{
    //This class is Heleper to Json Format Message
    public class MessageHelper : IMessageHelper
    {
        public List<string> GetOnlyNameOfSystem(List<string> listOfDevice)
        {
            List<string> uniqueSystems = listOfDevice
            .Select(sensor => sensor.Split('/')[0]) // Dzielimy string i bierzemy pierwszy element (nazwę systemu)
            .Distinct() // Usuwamy duplikaty, aby uzyskać unikalne nazwy systemów
            .ToList(); // Konwertujemy wynik na List<string>

            return uniqueSystems;
        }


        public void SplitIntoTwo(string input, out string part1, out string part2)
        {
            var index = input.IndexOf('/');
            if (index == -1)
            {
                part1 = input;
                part2 = null; // Brak '/', druga część jest null
            }
            else
            {
                part1 = input.Substring(0, index);
                part2 = input.Substring(index + 1);
            }
        }

    }
}
