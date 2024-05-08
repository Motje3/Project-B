using ReservationSystem;

public class MenuPresentation : View
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
            WriteLine("Your current tour reservation is:");
            MenuLogic._printTourString(GuidedTour.FindTourById(visitor.AssingedTourId));
            WriteLine("\nPlease choose an option:");
            WriteLine("1. Change my reservation time");
            WriteLine("2. Cancel my tour reservation");
            WriteLine("3. Continue to next visitor");
            Write("\nEnter your choice: ");
            string choice = ReadLine();

            choosingOption = logic.HandleFullMenuChoice(choice, visitor);
            

        
        }
    }
}
