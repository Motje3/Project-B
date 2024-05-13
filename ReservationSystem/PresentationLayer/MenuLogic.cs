public class MenuPresentation
{
    public static void ShowRestrictedMenu(Visitor visitor)
    {
        MenuLogic.JoinTour(visitor);
    }

    public static void ShowFullMenu(Visitor visitor)
    {
        MenuLogic logic = new MenuLogic();
        bool choosingOption = true;
        while (choosingOption)
        {
            string reservationDetails = Visitor.GetCurrentReservation(visitor);
            Console.WriteLine(reservationDetails);
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Change my reservation time");
            Console.WriteLine("2. Cancel my tour reservation");
            Console.WriteLine("3. Return to main menu");
            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            choosingOption = logic.HandleFullMenuChoice(choice, visitor);

        }
    }
}
