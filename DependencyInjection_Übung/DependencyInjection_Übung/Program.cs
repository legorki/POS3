
using System.Drawing;

class Point
{
    public double Lat { get; }
    public double Lng { get; }

    public Point(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }
}

interface ILocationProvider
{
    DateTime LastMeasurement { get; }
    Point GetLocation();
}
class AppleLocationProvider : ILocationProvider
{
    private Point? _currentPoint;
    public DateTime LastMeasurement { get; private set; } = DateTime.MinValue;
    public Point GetLocation()
    {
        // DEVICE SPECIFIC CODE

        Random rnd = new Random();
        DateTime now = DateTime.Now;
        if ((now - LastMeasurement).TotalSeconds > 5)
        {
            LastMeasurement = now;
            _currentPoint = new Point(rnd.NextDouble() * 180 - 90, rnd.NextDouble() * 360);
        }
        // Die Nullable analyse schlägt hier fehl. Wenn noch nie gemessen wurde, ist LastMeasurement
        // der 1.1.0001 und daher wird sicher ein Wer generiert. Mit NULL forgiving (!) sagen
        // wir, dass wir es besser wissen.
        return _currentPoint!;
    }
}

class AndroidLocationProvider : ILocationProvider
{
    private Point? _currentPoint;
    public DateTime LastMeasurement { get; private set; } = DateTime.MinValue;
    public Point GetLocation()
    {
        // DEVICE SPECIFIC CODE

        Random rnd = new Random();
        DateTime now = DateTime.Now;
        if ((now - LastMeasurement).TotalSeconds > 2)
        {
            LastMeasurement = now;
            _currentPoint = new Point(rnd.NextDouble() * 180 - 90, rnd.NextDouble() * 360);
        }
        return _currentPoint!;
    }
}

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}

public class FileLogger : ILogger
{
    private readonly string fileName;

    public FileLogger(string fileName)
    {
        this.fileName = fileName;
    }

    public void Log(string message)
    {
        System.IO.File.AppendAllText(fileName, message + Environment.NewLine);
    }
}

class MyTracker
{
    private readonly ILocationProvider locator;
    public ILogger Logger { get; set; } 

    public MyTracker(ILocationProvider locator)
    {
        this.locator = locator;
    }

    public void DoTracking(int seconds)
    {
        DateTime start = DateTime.Now;
        while ((DateTime.Now - start).TotalSeconds < seconds)
        {
            Point position = locator.GetLocation();
            Logger?.Log($"Lat: {position.Lat:0.00}°, Lng: {position.Lng:0.00}°");
            System.Threading.Thread.Sleep(1000);
        }
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Tracking mit ANDROID:");
        MyTracker tracker = new MyTracker(new AndroidLocationProvider());
        tracker.Logger = new ConsoleLogger();
        tracker.DoTracking(10);

        Console.WriteLine("Tracking mit APPLE:");
        tracker = new MyTracker(new AppleLocationProvider());
        tracker.Logger = new FileLogger("locations.txt");
        tracker.DoTracking(10);
    }
}
