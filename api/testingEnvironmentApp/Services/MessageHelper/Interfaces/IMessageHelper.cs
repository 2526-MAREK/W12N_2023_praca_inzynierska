using Newtonsoft.Json.Linq;

namespace testingEnvironmentApp.Services.MessageHelper.Interfaces
{
    public interface IMessageHelper
    {
        public List<string> GetOnlyNameOfSystem(List<string> listOfDevice);
        public void SplitIntoTwo(string input, out string part1, out string part2);
    }
}
