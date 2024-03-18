public static class ExampleScreen
{
    public static void ShowExample()
    {
        List<string> KeyboardPreset1 = new() { "keyboard", "3", "2", "10" };
        List<string> KeyboardPreset2 = new() { "keyboard", "1", "2", "5" };
        List<string> Box1Text = new() { "+-+", "|1|", "+-+" };
        List<string> Box2Text = new() { "+-+", "|2|", "+-+" };
        List<string> Box3Text = new() { "+-+", "|3|", "+-+" };
        List<string> Box4Text = new() { "+-+", "|4|", "+-+" };
        List<string> Box5Text = new() { "+-+", "|5|", "+-+" };
        List<string> Box6Text = new() { "+-+", "|6|", "+-+" };
        List<string> Box7Text = new() { "+-+", "|7|", "+-+" };
        List<string> Box8Text = new() { "+-+", "|8|", "+-+" };
        List<string> Box9Text = new() { "+-+", "|9|", "+-+" };
        SelectionConsoleObject Box1 = new(10, 5, Box1Text);
        SelectionConsoleObject Box2 = new(20, 5, Box2Text);
        SelectionConsoleObject Box3 = new(30, 5, Box3Text);
        SelectionConsoleObject Box6 = new(10, 12, Box6Text);
        SelectionConsoleObject Box7 = new(20, 12, Box7Text);
        SelectionConsoleObject Box8 = new(30, 12, Box8Text);
        SelectionConsoleObject Box9 = new(20, 15, Box9Text);
        SelectionConsoleObject Keyboard1 = new(10, 10, KeyboardPreset1);
        SelectionConsoleObject Keyboard2 = new(30, 10, KeyboardPreset2);

        List<SelectionConsoleObject> Row0 = new() { null, null, null, null, null };
        List<SelectionConsoleObject> Row1 = new() { null, Box1, Box2, Box3, null };
        List<SelectionConsoleObject> Row2 = new() { null, Keyboard1, null, Keyboard2, null };
        List<SelectionConsoleObject> Row3 = new() { null, Box6, Box7, Box8, null };
        List<SelectionConsoleObject> Row4 = new() { null, null, Box9, null, null };
        List<SelectionConsoleObject> Row5 = new() { null, null, null, null, null }
        ;
        List<List<SelectionConsoleObject>> SelectionBoxs = new() { Row0, Row1, Row2, Row3, Row4, Row5 };

        List<string> TextBox1Text = new() { "+-----+", "|Hello|", "+-----+" };
        List<StaticConsoleObject> TextBoxs = new() { new(20, 1, TextBox1Text) };

        Screen currentScreen = new(TextBoxs, SelectionBoxs, 1, 2,null,null);
        currentScreen.show();
    }
}