namespace Myrmidon.App;

public static class Program {
    
    [STAThread]
    private static void Main() {

        using var app = AppHost.Create();
        app.Run();

    }
}