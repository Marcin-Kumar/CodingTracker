
namespace CodingTracker
{
    internal class CodingTrackerManager
    {
        private readonly CodingTrackerController _codingController;

        public CodingTrackerManager(CodingTrackerController codingController)
        {
            _codingController = codingController;
        }

        internal void RunCodingTracker()
        {
            bool exitApp = false;
            while (!exitApp)
            {
                _codingController.ShowMenu();
                char menuOption = _codingController.GetMenuOption();
                Console.WriteLine();
                try
                {
                    switch (menuOption)
                    {
                        case 'i':
                            _codingController.ExecuteInsertProcess();
                            break;
                        case 'r':
                            _codingController.ExecuteDeleteProcess();
                            break;
                        case 'u':
                            _codingController.ExecuteUpdateProcess();
                            break;
                        case 'v':
                            _codingController.ExecuteViewingProcess();
                            break;
                        case 'e':
                            exitApp = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option entered");
                            break;
                    }
                }
                catch (ArgumentException ex){
                    Console.WriteLine($"{ex.Message}\n");
                }
            }
        }
    }
}